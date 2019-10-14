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
        /// コンストラクタ
        /// </summary>
        public RequiredH5Attribute()
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
            if (string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        /// <summary>
        /// クライアントでのバリデーション用の操作
        /// </summary>
        /// <param name="context">クライアントのバリデーションコンテキスト</param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // 以下のタグ属性を設定する
            // required                         "required"
            // notupported-required-err-msg     未サポートブラウザでのエラーメッセージ
            // required-err-msg                 バリデーションで設定されたエラーメッセージ
            MergeAttribute(context.Attributes, "required", "required");
            MergeAttribute(context.Attributes, "notsupported-required-err-msg", context.ModelMetadata.DisplayName + "は入力必須です。");
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "required-err-msg", ErrorMessage);
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
                return displayName + "は入力必須です。";
            }
            else
            {
                return ErrorMessage;
            }
        }
    }
}
