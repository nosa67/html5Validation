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
        /// バリデーション（サーバーサイド）
        /// クライアントバリデーションが外せなかったのでここはテストできてません
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
                        return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                    }
                }
                else
                {
                    // 文字列でなければエラー
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                }
            }
        }

        /// <summary>
        /// クライアントでのバリデーションのための操作
        /// </summary>
        /// <param name="context"></param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // TagHelperの影響で、type属性が「email」になっている
            // 以下のタグ属性を設定する
            // email-err-msg                    未サポートブラウザでのエラーメッセージ
            // typemis-err-msg                  バリデーションで設定されたエラーメッセージ(サポートされている場合で独自エラーメッセージが設定されている場合)
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MergeAttribute(context.Attributes, "email-err-msg", "メールアドレスになっていません。");
            }
            else
            {
                MergeAttribute(context.Attributes, "email-err-msg", ErrorMessage);
                MergeAttribute(context.Attributes, "typemis-err-msg", ErrorMessage);
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
                return displayName + "がメールアドレスになっていません。";
            }
            else
            {
                return ErrorMessage;
            }
        }
    }
}
