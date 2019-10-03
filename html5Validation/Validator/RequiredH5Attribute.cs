using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// 入力必須バリデーション
    /// </summary>
    public class RequiredH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        /// <summary>
        /// コンストラクタ（引数無）
        /// </summary>
        public RequiredH5Attribute()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RequiredH5Attribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RequiredH5Attribute(string errorMessage) : base(errorMessage)
        {
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
            if (value == null) return new ValidationResult(ErrorMessage);

            if (value.ToString().Trim().Length > 0)
            {
                // 入力の桁数（トリムしたあと）が1以上なら正常
                return ValidationResult.Success;
            }
            else
            {
                // 入力の桁数（トリムしたあと）が0以下なら異常
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

            // タグに「required="required"」と「required-err-msg="<エラーメッセジ>"」を設定する
            MergeAttribute(context.Attributes, "required", "required");
            if(!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "required-err-msg", ErrorMessage);
        }
    }
}
