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
        const string DEFAULT_RERORMSG = "日付が正しくありません。";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DateH5Attribute() : base(DataType.Date, DEFAULT_RERORMSG)
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
                    if (work.Hour != 0) return new ValidationResult(ErrorMessageString);
                    if (work.Minute != 0) return new ValidationResult(ErrorMessageString);
                    if (work.Second != 0) return new ValidationResult(ErrorMessageString);
                    if (work.Millisecond != 0) return new ValidationResult(ErrorMessageString);

                    // 日付だけなので正常
                    return ValidationResult.Success;
                }
                else
                {
                    // 日時でなければエラー
                    return new ValidationResult(ErrorMessageString);
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
            if (!string.IsNullOrWhiteSpace(ErrorMessageString)) MergeAttribute(context.Attributes, "date-err-msg", ErrorMessageString);
        }


    }
}
