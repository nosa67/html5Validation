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

        public string PatternName { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="RegularExpression">正規表現</param>
        public RegularExpressionH5Attribute(string RegularExpression)
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
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }
        }

        /// <summary>
        /// クライアントでのバリデーション用の操作
        /// </summary>
        /// <param name="context">クライアントのバリデーションコンテキスト</param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)　throw new ArgumentNullException(nameof(context));

            // 以下のタグ属性を設定する
            // pattern                          正規表現
            // notsupported-regular-err-msg     未サポートブラウザでのエラーメッセージ
            // regular-err-msg                  バリデーションで設定されたエラーメッセージ
            MergeAttribute(context.Attributes, "pattern", _regularExpression);
            MergeAttribute(context.Attributes, "notsupported-regular-err-msg", "入力が正しい形式では有りません。");
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                MergeAttribute(context.Attributes, "regular-err-msg", ErrorMessage);
            }
            else
            {
                if (!string.IsNullOrEmpty(PatternName))
                {
                    MergeAttribute(context.Attributes, "regular-err-msg", "入力が正しい形式(" + PatternName + ")では有りません。");
                }
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
                if (string.IsNullOrEmpty(PatternName))
                {
                    return "[" + displayName + "]の入力が正しい形式では有りません。";
                }
                else
                {
                    return "[" + displayName + "]の入力が正しい形式(" + PatternName + ")では有りません。";
                }
            }
            else
            {
                return ErrorMessage;
            }
        }
    }
}
