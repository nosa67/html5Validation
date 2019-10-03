using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace html5Validation.Validator
{
    /// <summary>
    /// HTML5バリデーション用データ種別アトリビュートの抽象クラス
    /// </summary>
    public abstract class DataTypeH5Attribute : DataTypeAttribute
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dataType"></param>
        protected DataTypeH5Attribute(DataType dataType) : base(dataType) { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessageAccessor">エラーメッセージへのアクセサ</param>
        protected DataTypeH5Attribute(DataType dataType, Func<string> errorMessageAccessor) : base(dataType)
        {
            ErrorMessage = errorMessageAccessor();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        protected DataTypeH5Attribute(DataType dataType, string errorMessage) : base(dataType)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// アトリビュートの設定
        /// </summary>
        /// <param name="attributes">アトリビュート</param>
        /// <param name="key">追加するキー</param>
        /// <param name="value">追加する値</param>
        protected void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {

            if (attributes.ContainsKey(key))
            {
                // 属性が既に設定されている場合

                if (string.IsNullOrWhiteSpace(attributes[key]))
                {
                    // 値が未設定の場合は値の未設定する
                    attributes[key] = value;
                }
                else
                {
                    // 値が設定されており、設定しようとする値と異なる場合は例外を発生させる                    
                    if (attributes[key] != value)
                    {
                        throw new ValidationH5Error("タグの属性[" + key + "]に設定されている値[" + attributes[key] + "]が想定[" + value + "]と異なります。");
                    }
                }
            }
            else
            {
                // 属性が未設定の場合は、新たな属性を設定する
                attributes.Add(key, value);
            }
        }


    }
}
