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
        public Object Min { get; set; } = null;
        public Object Max { get; set; } = null;
        public Object Step { get; set; } = null;

        public string UnderMinErrorMessage { get; set; }
        public string OverMaxErrorMessage { get; set; }
        public string NoStepErrorMessage { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Minimum">Int型での最小値</param>
        /// <param name="Maximum">Int型での最大値</param>
        public RangeH5Attribute()
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
                var type = value.GetType();
                var nullAbleType = Nullable.GetUnderlyingType(type);
                if (nullAbleType != null) type = nullAbleType;

                

                if ((type == typeof(sbyte)) || (type == typeof(short)) || (type == typeof(int)) ||
                    (type == typeof(long)) || (type == typeof(byte)) || (type == typeof(ushort)) || (type == typeof(uint)))
                {
                    // 整数(ulong以外)の場合
                    long longValue = (dynamic)value;

                    // 最小値チェック
                    if (Min != null)
                    {
                        long longMin = (dynamic)Min;

                        if (longValue < longMin) return new ValidationResult(GetMinErrorMessage(validationContext.DisplayName));
                    }
                     
                    // 最大値チェック
                    if (Max != null)
                    {
                        long longMax = (dynamic)Max;
                        if (longValue > longMax) return new ValidationResult(GetMaxErrorMessage(validationContext.DisplayName));
                    }

                    // 刻み幅チェック
                    if (Step != null)
                    {
                        long longMin = (dynamic)Min;
                        long longStep = (dynamic)Step;

                        if (((longValue - longMin) % longStep) != 0) return new ValidationResult(GetStepErrorMessage(validationContext.DisplayName));
                    }

                    return ValidationResult.Success;
                }
                else if (type == typeof(ulong))
                {
                    // 整数（ulong）の場合（longを使うとオーバーフローするため）
                    ulong ulongValue = (dynamic)value;

                    // 最小値チェック
                    if (Min != null)
                    {
                        ulong ulongMin = (dynamic)Min;

                        if (ulongValue < ulongMin) return new ValidationResult(GetMinErrorMessage(validationContext.DisplayName));
                    }

                    // 最大値チェック
                    if (Max != null)
                    {
                        ulong ulongMax = (dynamic)Max;

                        if (ulongValue > ulongMax) return new ValidationResult(GetMaxErrorMessage(validationContext.DisplayName));
                    }

                    // 刻み幅チェック
                    if (Step != null)
                    {
                        ulong ulongMin = (dynamic)Min;
                        ulong ulongStep = (dynamic)Step;

                        if (((ulongValue - ulongMin) % ulongStep) != 0) return new ValidationResult(GetStepErrorMessage(validationContext.DisplayName));
                    }

                    return ValidationResult.Success;
                }
                else
                {
                    // 実数の場合
                    double doubleValue = (dynamic)value;


                    // 最小値チェック
                    if (Min != null)
                    {
                        double doubleMin = (dynamic)Min;

                        if (doubleValue < doubleMin) return new ValidationResult(GetMinErrorMessage(validationContext.DisplayName));
                    }

                    // 最大値チェック
                    if (Max != null)
                    {
                        double doubleMax = (dynamic)Max;

                        if (doubleValue > doubleMax) return new ValidationResult(GetMaxErrorMessage(validationContext.DisplayName));
                    }

                    // 刻み幅チェック
                    if (Step != null)
                    {
                        double doubleMin = (dynamic)Min;
                        double doubleStep = (dynamic)Step;

                        var toLongRate = GetRealToIntegerRate(doubleMin, doubleStep, doubleValue);
                        long minLong = (long)Math.Round(doubleMin * toLongRate);
                        long stepLong = (long)Math.Round(doubleStep * toLongRate);
                        long inputLong = (long)Math.Round(doubleValue * toLongRate);

                        if (((inputLong - minLong) % stepLong) != 0) return new ValidationResult(GetStepErrorMessage(validationContext.DisplayName));
                    }

                    return ValidationResult.Success;
                }
            }
        }

        /// <summary>
        /// クライアントでのバリデーションサポート
        /// </summary>
        /// <param name="context">バリデーションコンテキスト</param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // データ型とMin,Max,Stepの型チェック
            var numericType = CheckRangeValues(context.ModelMetadata.ModelType);

            if (Min != null)
            {
                // 最小値が設定されている場合以下のタグ属性を設定する
                // minlength                            最小桁数
                // notsupported-min-length-err-msg      未サポートブラウザでのエラーメッセージ
                // min-length-err-msg                   バリデーションで設定されたエラーメッセージ
                MergeAttribute(context.Attributes, "min", Min.ToString());
                MergeAttribute(context.Attributes, "numeric-type", numericType.ToString());
                MergeAttribute(context.Attributes, "notsupported-min-err-msg", "最小値「" + Min.ToString() + "」より小さいです。");
                if (!string.IsNullOrWhiteSpace(UnderMinErrorMessage)) MergeAttribute(context.Attributes, "min-err-msg", UnderMinErrorMessage);
            }

            if (Max != null)
            {
                // 最大値が設定されている場合以下のタグ属性を設定する
                // maxlength                            最大桁数
                // notsupported-max-length-err-msg      未サポートブラウザでのエラーメッセージ
                // max-length-err-msg                   バリデーションで設定されたエラーメッセージ
                MergeAttribute(context.Attributes, "max", Max.ToString());
                MergeAttribute(context.Attributes, "numeric-type", numericType.ToString());
                MergeAttribute(context.Attributes, "notsupported-max-err-msg", "値が最大値「" + Max.ToString() + "」を超えています。");
                if (!string.IsNullOrWhiteSpace(OverMaxErrorMessage)) MergeAttribute(context.Attributes, "max-err-msg", OverMaxErrorMessage);
            }

            if (Step != null)
            {
                // 最大値が設定されている場合以下のタグ属性を設定する
                // maxlength                            最大桁数
                // notsupported-max-length-err-msg      未サポートブラウザでのエラーメッセージ
                // max-length-err-msg                   バリデーションで設定されたエラーメッセージ
                MergeAttribute(context.Attributes, "step", Step.ToString());
                MergeAttribute(context.Attributes, "notsupported-step-err-msg", "値が刻み幅「" + Step.ToString() + "」に一致しません。");
                if (!string.IsNullOrWhiteSpace(NoStepErrorMessage)) MergeAttribute(context.Attributes, "step-err-msg", NoStepErrorMessage);
            }
        }

        enum NumericTypes
        {
            Integer,
            Real,
            Other
        }

        NumericTypes CheckNumberType(Type type)
        {
            if (type == typeof(sbyte)) return NumericTypes.Integer;
            if (type == typeof(short)) return NumericTypes.Integer;
            if (type == typeof(int)) return NumericTypes.Integer;
            if (type == typeof(long)) return NumericTypes.Integer;
            if (type == typeof(byte)) return NumericTypes.Integer;
            if (type == typeof(ushort)) return NumericTypes.Integer;
            if (type == typeof(uint)) return NumericTypes.Integer;
            if (type == typeof(ulong)) return NumericTypes.Integer;
            if (type == typeof(float)) return NumericTypes.Real;
            if (type == typeof(double)) return NumericTypes.Real;
            if (type == typeof(decimal)) return NumericTypes.Real;

            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
            {
                if (nullableType == typeof(sbyte)) return NumericTypes.Integer;
                if (nullableType == typeof(short)) return NumericTypes.Integer;
                if (nullableType == typeof(int)) return NumericTypes.Integer;
                if (nullableType == typeof(long)) return NumericTypes.Integer;
                if (nullableType == typeof(byte)) return NumericTypes.Integer;
                if (nullableType == typeof(ushort)) return NumericTypes.Integer;
                if (nullableType == typeof(uint)) return NumericTypes.Integer;
                if (nullableType == typeof(ulong)) return NumericTypes.Integer;
                if (nullableType == typeof(float)) return NumericTypes.Real;
                if (nullableType == typeof(double)) return NumericTypes.Real;
                if (nullableType == typeof(decimal)) return NumericTypes.Real;
            }

            return NumericTypes.Other;
        }

        NumericTypes CheckRangeValues(Type type)
        {
            var numericType = CheckNumberType(type);
            if (numericType == NumericTypes.Other) throw new ValidationException("Range属性はデータの型(" + type.Name + ")では使用できません。");

            var nullAbleType = Nullable.GetUnderlyingType(type);
            if (nullAbleType != null) type = nullAbleType;

            if (Min != null)
            {
                try
                {
                    var work = Convert.ChangeType(Min, type);
                }
                catch
                {
                    throw new ValidationException("Range属性の[Min]の値(" + Min.ToString() + ")が対象のデータの型(" + type .Name + ")では使用できません。");
                }
            }

            if (Max != null)
            {
                try
                {
                    var work = Convert.ChangeType(Max, type);
                }
                catch
                {
                    throw new ValidationException("Range属性の[Max]の値(" + Max.ToString() + ")が対象のデータの型(" + type.Name + ")では使用できません。");
                }
            }

            if (Step != null)
            {
                if (Min == null) throw new ValidationException("Range属性の[Step]は[Min]無では使用できません。");

                try
                {
                    var work = Convert.ChangeType(Step, type);
                }
                catch
                {
                    throw new ValidationException("Range属性の[Step]の値(" + Step.ToString() + ")が対象のデータの型(" + type.Name + ")では使用できません。");
                }
            }

            return numericType;
        }

        string GetMinErrorMessage(string displayName)
        {
            if(string.IsNullOrWhiteSpace(UnderMinErrorMessage))
            {
                return "[" + displayName + "]は最小値「" + Min.ToString() + "」より小さいです。";
            }
            else
            {
                return UnderMinErrorMessage;
            }
        }

        string GetMaxErrorMessage(string displayName)
        {
            if (string.IsNullOrWhiteSpace(OverMaxErrorMessage))
            {
                return "[" + displayName + "]は最大値「" + Max.ToString() + "」より大きいです。";
            }
            else
            {
                return OverMaxErrorMessage;
            }
        }

        string GetStepErrorMessage(string displayName)
        {
            if (string.IsNullOrWhiteSpace(NoStepErrorMessage))
            {
                return "[" + displayName + "]は刻み幅「" + Step.ToString() + "」が一致しません。";
            }
            else
            {
                return NoStepErrorMessage;
            }
        }

        long GetRealToIntegerRate(double minValue, double stepValue, double inputValue)
        {
            var minStr = minValue.ToString();
            var stepStr = stepValue.ToString();
            var inputStr = inputValue.ToString();
            var splitter = new char[]{ '.'};

            var under1Length = 0;
            var minSplitterdStr = minStr.Split(splitter);
            if(minSplitterdStr.Length == 2)
            {
                if (under1Length < minSplitterdStr[1].Length) under1Length = minSplitterdStr[1].Length;
            }

            var stepSplitterdStr = stepStr.Split(splitter);
            if (stepSplitterdStr.Length == 2)
            {
                if (under1Length < stepSplitterdStr[1].Length) under1Length = stepSplitterdStr[1].Length;
            }

            var inputSplitterdStr = inputStr.Split(splitter);
            if (inputSplitterdStr.Length == 2)
            {
                if (under1Length < inputSplitterdStr[1].Length) under1Length = inputSplitterdStr[1].Length;
            }

            long result = 1L;
            for (int i = 0; i < under1Length; i++) result *= 10L;
            return result;
        }
    }
}
