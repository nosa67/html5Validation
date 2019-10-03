using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// 指定した項目と同じかどうかを検証するバリデーション
    /// （例：パスワードとパスワードの確認が一致すること）
    /// </summary>
    public class CompareH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        // デフォルトのエラーメッセージ（HTML5のバリデーションにはないのでデフォルトメッセージはここで設定している）
        string defauleErrMessage = "項目%%%と値が一致しません。";

        // 比較対象の項目名
        string _compareItemId = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="compareItemId">比較対象の項目名</param>
        public CompareH5Attribute(string compareItemId)
        {
            _compareItemId = compareItemId;
            ErrorMessage = defauleErrMessage.Replace("%%%", compareItemId);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="compareItemId">比較対象の項目名</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public CompareH5Attribute(string compareItemId, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _compareItemId = compareItemId;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="compareItemId">比較対象の項目名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public CompareH5Attribute(string compareItemId, string ErrorMessage) : base(ErrorMessage)
        {
            _compareItemId = compareItemId;
        }

        /// <summary>
        /// バリデーション（サーバーサイド）
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="validationContext">バリデーションコンテキスト</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // 比較対象の項目名をモデルから取得する
            var prop = validationContext.ObjectInstance.GetType().GetProperty(_compareItemId);
            if(prop == null) throw new ValidationH5Error("Compareバリデーションで設定されている対象の項目" + _compareItemId + "がモデル内に存在しません。)");

            // 比較対象の値を取得する
            var targetValue = prop.GetValue(validationContext.ObjectInstance);

            // 比較対象の値とこの値の型が異なる場合はその旨の例外を発生させる
            if (value.GetType() != targetValue.GetType()) throw new ValidationH5Error("Compareバリデーションで設定されている対象のデータ型が異なります。(" + value.GetType().Name + "," + validationContext.ObjectInstance.GetType().Name +  ")");

            // 比較対象の項目の値を取得し、この項目の値（value）と比較している。
            // 基本的にはクライアントサイドで実施されるが、ここでは直接POSTされた場合の対処
            
            if (value == null)
            {
                if (prop.GetValue(validationContext.ObjectInstance) == null)
                {
                    // この項目と比較対象の項目がともにnullなら同一
                    return ValidationResult.Success;
                }
                else
                {
                    //  この項目がnullで比較対象がnull以外なので不一致エラーを返す
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                if(targetValue == null)
                {
                    // この項目がnull以外で比較項目がnullなので不一致エラーを返す
                    return new ValidationResult(ErrorMessage);
                }
                else
                {
                    if (value.GetType().IsClass)
                    {
                        // プリミティブ型以外は比較できるかどうか調べる
                        if (value.GetType() is IComparable)
                        {
                            // 比較メソッドで比較実施
                            if (((IComparable)value).CompareTo(targetValue) == 0)
                            {
                                return ValidationResult.Success;
                            }
                            else
                            {
                                return new ValidationResult(ErrorMessage);
                            }
                        }
                        else
                        {
                            throw new ValidationH5Error("Compareバリデーションで設定されている対象のデータ型は比較できません。(IComparableインタフェースが必要)");
                        }

                    }
                    else
                    {
                        // プリミティブ型の場合は単純比較を行う
                        if (value == targetValue)
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult(ErrorMessage);
                        }

                        
                    }
                }
            }
        }

        /// <summary>
        /// クライアントでのバリデーションのための操作
        /// </summary>
        /// <param name="context"></param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // 比較対象のタグのIDを取得する（idの性質が）
            var targetId = context.Attributes["id"].Replace(context.ModelMetadata.Name, _compareItemId);

            // 比較対象のエレメントIDを「compare-item-id」属性に、エラーメッセージを「compare-err-msg」属性に設定。これはクライアントのJavaScriptで処理される
            MergeAttribute(context.Attributes, "compare-item-id", targetId);
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "compare-err-msg", ErrorMessage);
        }
    }
}
