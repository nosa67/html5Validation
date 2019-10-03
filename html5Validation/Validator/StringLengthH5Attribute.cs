using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// 文字列長バリデーション
    /// </summary>
    public class StringLengthH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        /// <summary>
        /// 最小長 -1 ならチェック対象外
        /// </summary>
        int _minLength = 0;

        /// <summary>
        /// 最大長　-1 ならチェック対処具合
        /// </summary>
        int _maxLength = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="MaxLength">最大長</param>
        /// <param name="MinLength">最小長</param>
        public StringLengthH5Attribute(int MaxLength, int MinLength = 0)
        {
            _minLength = MinLength;
            _maxLength = MaxLength;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="MaxLength">最大長</param>
        /// <param name="MinLength">最小長</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public StringLengthH5Attribute(int MaxLength, Func<string> errorMessageAccessor, int MinLength = 0) : base(errorMessageAccessor)
        {
            _minLength = MinLength;
            _maxLength = MaxLength;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public StringLengthH5Attribute(int MaxLength, string errorMessage, int MinLength =0) : base(errorMessage)
        {
            _minLength = MinLength;
            _maxLength = MaxLength;
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
            // 入力が空の場合は常に正常（空のチェックは入力必須でおこなう）
            if (value == null)　return ValidationResult.Success;

            // 最小桁数チェック
            if ((_minLength > 0)  && (value.ToString().Trim().Length < _minLength))
            {
                return new ValidationResult(ErrorMessage);
            }

            // 最大桁数チェック
            if (value.ToString().Trim().Length > _maxLength)
            {
                // 入力の桁数（トリムしたあと）が1以上なら正常
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
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
            if (_minLength >0) MergeAttribute(context.Attributes, "minlength", _minLength.ToString());
            if (_maxLength > 0) MergeAttribute(context.Attributes, "maxlength", _maxLength.ToString());
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "stringLength-err-msg", ErrorMessage);
        }
    }
}
