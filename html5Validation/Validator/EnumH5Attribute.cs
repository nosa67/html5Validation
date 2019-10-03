using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// enum型の入力チェック
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
            var msg = "";
            foreach (var enumOne in Enum.GetValues(EnumType))
            {
                msg += "," + enumOne.ToString();
            }
            ErrorMessage = msg.Substring(1) + "のいずれかを入力してください。";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public EnumH5Attribute(Type EnumType, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _enumType = EnumType;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public EnumH5Attribute(Type EnumType, string errorMessage) : base(errorMessage)
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
            //  入寮必須はここではチェック対象外
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
                    return new ValidationResult(ErrorMessage);
                }
            }
            else
            {
                return new ValidationResult(ErrorMessage);
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

            var enumValues = "";
            foreach (var enumItem in Enum.GetValues(_enumType)) enumValues += "," + enumItem.ToString();
            enumValues = enumValues.Substring(1);

            // タグに「required="required"」と「required-err-msg="<エラーメッセジ>"」を設定する
            MergeAttribute(context.Attributes, "enum-values", enumValues);
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "enum-err-msg", ErrorMessage);
        }
    }
}
