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
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public DateH5Attribute(Func<string> errorMessageAccessor) : base(DataType.Date, errorMessageAccessor)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        public DateH5Attribute(string errorMessage) : base(DataType.Date, errorMessage)
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
                    if (work.Hour != 0) return new ValidationResult(ErrorMessage);
                    if (work.Minute != 0) return new ValidationResult(ErrorMessage);
                    if (work.Second != 0) return new ValidationResult(ErrorMessage);
                    if (work.Millisecond != 0) return new ValidationResult(ErrorMessage);

                    // 日付だけなので正常
                    return ValidationResult.Success;
                }
                else
                {
                    // 日時でなければエラー
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
