using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace html5Validation.Validator
{
    /// <summary>
    /// 範囲指定入力バリデーション
    /// htmlタグでのtypeは自身で指定すること（text, numer,rangeが設定可能） 
    /// </summary>
    public class RangeH5Attribute : ValidationH5Attributecs, IClientModelValidator
    {
        int? _minInt = null;
        int? _maxInt = null;
        int? _stepInt = null;

        double? _minDouble = null;
        double? _maxDouble = null;
        double? _stepDouble = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        public RangeH5Attribute(int Minimum, int Maximum)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        /// <param name="Step">Int型でのステップ</param>
        public RangeH5Attribute(int Minimum, int Maximum, int Step)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
            _stepInt = Step;
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RangeH5Attribute(int Minimum, int Maximum, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        /// <param name="Step">Int型でのステップ</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RangeH5Attribute(int Minimum, int Maximum, Func<string> errorMessageAccessor, int Step) : base(errorMessageAccessor)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
            _stepInt = Step;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RangeH5Attribute(int Minimum, int Maximum, string errorMessage) : base(errorMessage)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        /// <param name="Step">Int型でのステップ</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RangeH5Attribute(int Minimum, int Maximum, string errorMessage, int Step) : base(errorMessage)
        {
            _minInt = Minimum;
            _maxInt = Maximum;
            _stepInt = Step;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        public RangeH5Attribute(double Minimum, double Maximum)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = 0.01;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        public RangeH5Attribute(double Minimum, double Maximum, double Step)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = Step;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RangeH5Attribute(double Minimum, double Maximum, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = 0.01;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        public RangeH5Attribute(double Minimum, double Maximum, double Step, Func<string> errorMessageAccessor) : base(errorMessageAccessor)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = Step;
        }

        /// <summary>   
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RangeH5Attribute(double Minimum, double Maximum, string errorMessage) : base(errorMessage)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = 0.01;
        }

        /// <summary>   
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">double型での最小値</param>
        /// <param name="Maximum">double型での最大値</param>
        /// <param name="Step">doble型でのステップ</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        public RangeH5Attribute(double Minimum, double Maximum, double Step, string errorMessage) : base(errorMessage)
        {
            _minDouble = Minimum;
            _maxDouble = Maximum;
            _stepDouble = Step;
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
                if (_minInt != null)
                {
                    // intの場合のチェック
                    if (value.GetType() != typeof(int)) throw new ValidationH5Error("整数(int)の入力チェックが行われていますが、入力が整数ではありません。（" + value.GetType().Name + "）");

                    var intValue = (int)value;
                    if (intValue < _minInt) return new ValidationResult(ErrorMessage);    // 最小値未満の場合はエラー
                    if (intValue > _maxInt) return new ValidationResult(ErrorMessage);    // 最小値未満の場合はエラー
                    if (_stepInt != null)
                    {
                        var amari = (intValue - _minInt) % _stepInt;
                        if (amari != 0) return new ValidationResult(ErrorMessage);
                    }
                }
                else
                {
                    // doubleの場合のチェック
                    if (value.GetType() != typeof(double)) throw new ValidationH5Error("実数(double)の入力チェックが行われていますが、入力が実数ではありません。（" + value.GetType().Name + "）");

                    var doubleValue = (double)value;
                    if (doubleValue < _minDouble) return new ValidationResult(ErrorMessage);    // 最小値未満の場合はエラー
                    if (doubleValue > _maxDouble) return new ValidationResult(ErrorMessage);    // 最小値未満の場合はエラー
                    if (_stepDouble != null)
                    {
                        double divValue = (doubleValue - (double)_minDouble) / (double)_stepInt;
                        if ((divValue - (int)divValue) != 0) return new ValidationResult(ErrorMessage);
                    }
                }

                return ValidationResult.Success;
            }
        }

        /// <summary>
        /// クライアントでのバリデーションサポート
        /// </summary>
        /// <param name="context">バリデーションコンテキスト</param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // タグに「required="required"」と「required-err-msg="<エラーメッセジ>"」を設定する
            if (_minInt != null)
            {
                MergeAttribute(context.Attributes, "min", _minInt.ToString());
                MergeAttribute(context.Attributes, "max", _maxInt.ToString());
                if(_stepInt != null) MergeAttribute(context.Attributes, "step", _stepInt.ToString());
            }
            else
            {
                MergeAttribute(context.Attributes, "min", _minDouble.ToString());
                MergeAttribute(context.Attributes, "max", _maxDouble.ToString());
                if (_stepDouble != null) MergeAttribute(context.Attributes, "step", _stepDouble.ToString());
            }
            MergeAttribute(context.Attributes, "type", "range");
            if (!string.IsNullOrWhiteSpace(ErrorMessage)) MergeAttribute(context.Attributes, "range-err-msg", ErrorMessage);
            
        }
    }
}
