using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace html5Validation.Validator
{
    /// <summary>
    /// 正規表現バリデーション
    /// </summary>
    public class RegularExpressionH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        /// <summary>
        /// 正規表現
        /// </summary>
        string _regularExpression = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="RegularExpression">正規表現</param>
        public RegularExpressionH5Attribute(string RegularExpression)
        {
            _regularExpression = RegularExpression;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="RegularExpression">正規表現</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RegularExpressionH5Attribute(string RegularExpression, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _regularExpression = RegularExpression;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="RegularExpression">正規表現</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RegularExpressionH5Attribute(string RegularExpression, string errorMessage) : base(errorMessage)
        {
            _regularExpression = RegularExpression;
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
            // nullはこのバリデーションでは正常を返す
            if (value == null) return ValidationResult.Success;

            // 正規表現チェック
            if (Regex.Match(value.ToString(), _regularExpression).Success)
            {
                return ValidationResult.Success;
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

            // タグに「required="required"」と「required-err-msg="<エラーメッセジ>"」を設定する
            MergeAttribute(context.Attributes, "pattern", _regularExpression);
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "regular-err-msg", ErrorMessage);
        }
    }
}
