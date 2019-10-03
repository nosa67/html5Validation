using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// HTML5バリデーション用メールアドレスアトリビュート
    /// 残念ながらメールアドレスの仕様は難しく、完全なチェックとはなっていない（html5でも正しい判定はできていない模様）
    /// </summary>
    public class EmailAddressH5Attribute : DataTypeH5Attribute, IClientModelValidator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmailAddressH5Attribute() : base(DataType.EmailAddress)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public EmailAddressH5Attribute(Func<string> errorMessageAccessor) : base(DataType.EmailAddress, errorMessageAccessor)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public EmailAddressH5Attribute(string errorMessage) : base(DataType.EmailAddress, errorMessage)
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
            if (value == null)
            {
                // 空白はエラーではない
                return ValidationResult.Success;
            }
            else
            {
                if (value.GetType() == typeof(string))
                {
                    try
                    {
                        // メールアドレスクラスを作成できるのなら正常を返す
                        System.Net.Mail.MailAddress dummy = new System.Net.Mail.MailAddress(value.ToString());
                        return ValidationResult.Success;
                    }
                    catch (FormatException)
                    {
                        // メールアドレスクラスを作成できないならエラー
                        return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    // 文字列でなければエラー
                    return new ValidationResult(ErrorMessage);
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

            // タグの「typemis-err-msg」属性にエラーメッセージを設定
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "typemis-err-msg", ErrorMessage);
        }


    }
}
