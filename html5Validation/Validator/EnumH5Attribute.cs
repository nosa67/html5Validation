using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// enum型の入力チェック
    /// inputによる入力は考えられないので、実際には使うことはないと思うが。
    /// </summary>
    public class EnumH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        Type _enumType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnumH5Attribute(Type EnumType)
        {
            _enumType = EnumType;
            
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
            //  入力必須はここではチェック対象外
            if (value == null) return ValidationResult.Success;

            if (value.GetType() == _enumType)
            {
                return ValidationResult.Success;
            }
            else if (value.GetType() == typeof(string))
            {
                object work;
                if (Enum.TryParse(_enumType, value.ToString(), out work))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }
            else
            {
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }
        }

        /// <summary>
        /// クライアントでのバリデーション用の操作
        /// </summary>
        /// <param name="context">クライアントのバリデーションコンテキスト</param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var enumValues = getTenumLists();

            // 以下のタグ属性を設定する
            // enum-values                      列挙のリスト
            // enum-err-msg                     エラーメッセージ
            MergeAttribute(context.Attributes, "enum-values", enumValues);
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MergeAttribute(context.Attributes, "enum-err-msg", getTenumLists() + "以外が入力されています。");
            }
            else
            {
                MergeAttribute(context.Attributes, "enum-err-msg", ErrorMessage);
            }
        }


        /// <summary>
        /// サーバーバリデーション時のエラーメッセージ取得
        /// </summary>
        /// <param name="displayName">表示名称（DisplayNameアトリビュートで変更できる）</param>
        /// <returns>必須エラーメッセージ</returns>
        string GetErrorMessage(string displayName)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                var msg = "";
                foreach (var enumOne in Enum.GetValues(_enumType))
                {
                    msg += "," + enumOne.ToString();
                }
                return "[" + displayName + "]に" + getTenumLists() + "以外が入力されています。";
            }
            else
            {
                return ErrorMessage;
            }
        }

        /// <summary>
        /// 列挙リストの作成
        /// </summary>
        /// <returns></returns>
        string getTenumLists()
        {
            var msg = "";
            foreach (var enumOne in Enum.GetValues(_enumType))
            {
                msg += "," + enumOne.ToString();
            }
            return msg.Substring(1);
        }
    }
}
