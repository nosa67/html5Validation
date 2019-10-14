using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace html5Validation.Validator
{
    /// <summary>
    /// 指定した項目と同じかどうかを検証するバリデーション
    /// （例：パスワードとパスワードの確認が一致すること）
    /// </summary>
    public class CompareH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        // 比較対象の項目名
        string _compareItemId = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="compareItemId">比較対象の項目名</param>
        public CompareH5Attribute(string compareItemId)
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
            var targetProp = validationContext.ObjectInstance.GetType().GetProperty(_compareItemId);
            if(targetProp == null) throw new ValidationH5Error("Compareバリデーションで設定されている対象の項目" + _compareItemId + "がモデル内に存在しません。)");

            // 比較対象の値を取得する
            var targetValue = targetProp.GetValue(validationContext.ObjectInstance);

            // 比較対象の項目の値を取得し、この項目の値（value）と比較している。
            // 基本的にはクライアントサイドで実施されるが、ここでは直接POSTされた場合の対処
            
            if (value == null)
            {
                if (targetProp.GetValue(validationContext.ObjectInstance) == null)
                {
                    // この項目と比較対象の項目がともにnullなら同一
                    return ValidationResult.Success;
                }
                else
                {
                    //  この項目がnullで比較対象がnull以外なので不一致エラーを返す
                    return new ValidationResult(GetServerErrorMessage(validationContext.DisplayName, targetProp));
                }
            }
            else
            {
                if(targetValue == null)
                {
                    // この項目がnull以外で比較項目がnullなので不一致エラーを返す
                    return new ValidationResult(ErrorMessageString);
                }
                else
                {
                    // 比較対象の値とこの値の型が異なる場合はその旨の例外を発生させる
                    if (value.GetType() != targetValue.GetType()) throw new ValidationH5Error("Compareバリデーションで設定されている対象のデータ型が異なります。(" + value.GetType().Name + "," + validationContext.ObjectInstance.GetType().Name + ")");

                    // 文字列にして比較（もう少し修正する必要があるかも）
                    if (value.ToString() == targetValue.ToString())
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(GetServerErrorMessage(validationContext.DisplayName, targetProp));
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

            var targetProp = context.ModelMetadata.ContainerType.GetProperty(_compareItemId);

            // 比較対象のタグのIDを取得する（idの性質が）
            var targetId = context.Attributes["id"].Replace(context.ModelMetadata.Name, _compareItemId);

            // 以下のタグ属性を設定する
            // compare-item-id      比較対象の項目名
            // compare-err-msg      バリデーションで設定されたエラーメッセージ（html5には個のバリデーションはないので、デフォルトエラーは存在しない）
            MergeAttribute(context.Attributes, "compare-item-id", targetId);
            if (!string.IsNullOrWhiteSpace(ErrorMessageString)) MergeAttribute(context.Attributes, "compare-err-msg", GetClientErrorMessage(targetProp));

        }

        string GetClientErrorMessage(PropertyInfo targetPorop)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                var targetDisplayName = Attribute.GetCustomAttributes(targetPorop, typeof(DisplayNameAttribute)) as DisplayNameAttribute[];

                return "[" + targetDisplayName[0].DisplayName + "]の内容と一致しません。";
            }
            else
            {
                return ErrorMessage;
            }
        }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        string GetServerErrorMessage(string displayName, PropertyInfo targetPorop)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                var targetDisplayName = Attribute.GetCustomAttributes(targetPorop, typeof(DisplayNameAttribute)) as DisplayNameAttribute[];

                return "[" + displayName + "]の内容が[" + targetDisplayName[0].DisplayName + "]の内容と一致しません。";
            }
            else
            {
                return ErrorMessage;
            }
        }
    }
}
