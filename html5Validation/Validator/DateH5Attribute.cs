using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// 日付のデータアノテーション
    /// </summary>
    public class DateH5Attribute : DataTypeH5Attribute, IClientModelValidator
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DateH5Attribute() : base(DataType.Date)
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
                if (value.GetType() == typeof(DateTime))
                {
                    // 時分秒ミリ秒が0以外はエラー
                    var work = (DateTime)value;
                    if (work.Hour != 0) return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                    if (work.Minute != 0) return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                    if (work.Second != 0) return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
                    if (work.Millisecond != 0) return new ValidationResult(GetErrorMessage(validationContext.DisplayName));

                    // 日付だけなので正常
                    return ValidationResult.Success;
                }
                else
                {
                    // 日時でなければエラー
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // TagHelperの影響で、type属性が「email」になっている
            // 以下のタグ属性を設定する
            // date-err-msg                    未サポートブラウザでのエラーメッセージ
            // typemis-err-msg                  バリデーションで設定されたエラーメッセージ(サポートされている場合で独自エラーメッセージが設定されている場合)
            if (string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MergeAttribute(context.Attributes, "date-err-msg", "日付になっていません。");
            }
            else
            {
                MergeAttribute(context.Attributes, "date-err-msg", ErrorMessage);
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
                return displayName + "が日付になっていません。";
            }
            else
            {
                return ErrorMessage;
            }
        }
    }
}
