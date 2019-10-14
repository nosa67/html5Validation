using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using html5Validation.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace html5Validation.Pages
{
    public class IndexModel : PageModel
    {
        public enum Fluits
        { 
            リンゴ,
            みかん,
            イチゴ,
            Glape,
            桃
        }


        [BindProperty]
        public TestInputs Inputs { get; set; }

        public class TestInputs
        {
            [RequiredH5]
            [DisplayName("必須")]
            public string RequireString { get; set; }

            [RequiredH5(ErrorMessage = "入力必須のエラーメッセージを変更")]
            [DisplayName("必須(メッセージ変更有)")]
            public string RequireStringWithMSG { get; set; }

            [CompareH5("RequireString")]
            [DisplayName("必須と比較")]
            public string CompareString { get; set; }

            [CompareH5("RequireString",ErrorMessage = "比較のエラーメッセージを変更")]
            [DisplayName("必須と比較(メッセージ変更有)")]
            public string CompareStringWithMSG { get; set; }

            [StringLengthH5(MinLength = 5)]
            [DisplayName("文字列長最小5文字")]
            public string StringLengthStringMin { get; set; }

            [StringLengthH5(MaxLength = 10)]
            [DisplayName("文字列長最大10文字")]
            public string StringLengthStringMax { get; set; }

            [StringLengthH5(MinLength = 5, MaxLength = 10)]
            [DisplayName("文字列長最小5文字、最大10文字")]
            public string StringLengthStringBoth { get; set; }

            [StringLengthH5(MinLength = 5, UnderMinErrorMessage = "最小値5以下のエラーメッセージ（変更）")]
            [DisplayName("文字列長最小5文字(メッセージ変更有)")]
            public string StringLengthStringMinWithMSG { get; set; }

            [StringLengthH5(MaxLength = 10, OverMaxErrorMessage = "最大値10以下のエラーメッセージ（変更）")]
            [DisplayName("文字列長最大10文字(メッセージ変更有)")]
            public string StringLengthStringMaxWithMSG { get; set; }

            [StringLengthH5(MinLength = 5, MaxLength = 10, UnderMinErrorMessage = "最小値5以下のエラーメッセージ（変更）", OverMaxErrorMessage = "最大値10以下のエラーメッセージ（変更）")]
            [DisplayName("文字列長最小5文字、最大10文字(メッセージ変更有)")]
            public string StringLengthStringBothWithMSG { get; set; }

            [EmailAddressH5()]
            [DisplayName("メールアドレス")]
            public string MailAddress { get; set; }

            [EmailAddressH5(ErrorMessage = "メールアドレスエラー(メッセージ変更有)")]
            [DisplayName("メールアドレス(メッセージ変更有)")]
            public string MailAddressWithMSG { get; set; }

            [EnumH5(typeof(Fluits))]
            [DisplayName("列挙（フルーツ：リンゴ,みかん,イチゴ,Glape,桃）")]
            public Fluits? Fluits { get; set; }

            [EnumH5(typeof(Fluits),ErrorMessage = "列挙エラー(メッセージ変更有)")]
            [DisplayName("列挙（フルーツ：リンゴ,みかん,イチゴ,Glape,桃）(メッセージ変更有)")]
            public Fluits? FluitsWithMSG { get; set; }
            

            [DateH5("これなんかどうなる？")]
            public DateTime? InputDay { get; set; }

            [RangeH5(Min = 1)]
            [DisplayName("整数最小1")]
            public int? IntRangeMin { get; set; }

            [RangeH5(Min = 1, Step =2)]
            [DisplayName("整数最小1ステップ2")]
            public int? IntRangeMinStep { get; set; }

            [RangeH5(Max = 5)]
            [DisplayName("整数最大5")]
            public int? IntRangeMax { get; set; }

            [RangeH5(Min = 2,Max = 6)]
            [DisplayName("整数最小2最大6")]
            public int? IntRangeBoth { get; set; }

            [RangeH5(Min = 2, Max = 6, Step = 2)]
            [DisplayName("整数最小2最大6ステップ2")]
            public int? IntRangeBothStep { get; set; }

            [RangeH5(Min = 1, UnderMinErrorMessage ="最小値エラー変更")]
            [DisplayName("整数最小1(メッセージ変更有)")]
            public int? IntRangeMinWithMSG { get; set; }

            [RangeH5(Min = 1, Step = 3, UnderMinErrorMessage = "最小値エラー変更", NoStepErrorMessage = "刻み幅エラー変更")]
            [DisplayName("整数最小1ステップ3(メッセージ変更有)")]
            public int? IntRangeMinStepWithMSG { get; set; }

            [RangeH5(Max = 5, OverMaxErrorMessage ="最大値エラー変更")]
            [DisplayName("整数最大5(メッセージ変更有)")]
            public int? IntRangeMaxWithMSG { get; set; }

            [RangeH5(Min = 2, Max = 6, UnderMinErrorMessage ="最小値エラー変更", OverMaxErrorMessage ="最大値エラー変更")]
            [DisplayName("整数最小2最大6(メッセージ変更有)")]
            public int? IntRangeBothWithMSG { get; set; }

            [RangeH5(Min = 2, Max = 6, Step = 3, UnderMinErrorMessage = "最小値エラー変更", OverMaxErrorMessage = "最大値エラー変更", NoStepErrorMessage = "刻み幅エラー変更")]
            [DisplayName("整数最小2最大6ステップ3(メッセージ変更有)")]
            public int? IntRangeBothStepWithMSG { get; set; }


            [RangeH5(Min = -1.2)]
            [DisplayName("実数最小-1.2")]
            public double? DoubleRangeMin { get; set; }

            [RangeH5(Min = -2, Step = 0.1)]
            [DisplayName("実数最小-2ステップ0.1")]
            public double? DoubleRangeMinStep { get; set; }

            [RangeH5(Max = 2.3)]
            [DisplayName("実数最大2.3")]
            public double? DoubleRangeMax { get; set; }

            [RangeH5(Min = -1.2, Max = 2.1)]
            [DisplayName("実数最小-1.2最大2.1")]
            public double? DoubleRangeBoth { get; set; }

            [RangeH5(Min = -1.2, Max = 2.5, Step = 0.05)]
            [DisplayName("実数最小-1.2最大2.5ステップ0.05")]
            public double? DoubleRangeBothStep { get; set; }

            [RangeH5(Min = -2, UnderMinErrorMessage = "最小値エラー変更")]
            [DisplayName("実数最小-2(メッセージ変更有)")]
            public double? DoubleRangeMinWithMSG { get; set; }

            [RangeH5(Min = -31.1, Step = 1, UnderMinErrorMessage = "最小値エラー変更", NoStepErrorMessage = "刻み幅エラー変更")]
            [DisplayName("実数最小-31.1ステップ1(メッセージ変更有)")]
            public double? DoubleRangeMinStepWithMSG { get; set; }

            [RangeH5(Max = 500, OverMaxErrorMessage = "最大値エラー変更")]
            [DisplayName("実数最大500(メッセージ変更有)")]
            public double? DoubleRangeMaxWithMSG { get; set; }

            [RangeH5(Min = -2.1, Max = 20, UnderMinErrorMessage = "最小値エラー変更", OverMaxErrorMessage = "最大値エラー変更")]
            [DisplayName("実数最小-2.1最大20(メッセージ変更有)")]
            public double? DoubleRangeBothWithMSG { get; set; }

            [RangeH5(Min = -22, Max = 6, Step = 0.3, UnderMinErrorMessage = "最小値エラー変更", OverMaxErrorMessage = "最大値エラー変更", NoStepErrorMessage = "刻み幅エラー変更")]
            [DisplayName("実数最小-22最大6ステップ0.3(メッセージ変更有)")]
            public double? DoubleRangeBothStepWithMSG { get; set; }

            [RegularExpressionH5(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
            [DisplayName("正規表現（メール形式）")]
            public string Regular{get; set;}

            [RegularExpressionH5(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", PatternName ="メール形式")]
            [DisplayName("正規表現（メール形式：パターン名指定）")]
            public string RegularPatternName { get; set; }

            [RegularExpressionH5(@"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "メール形式エラー（変更）")]
            [DisplayName("正規表現（メール形式、メッセージ変更有）")]
            public string RegularWithMSG { get; set; }
        }

        public string SuccessMesssage { get; set; }


        public void OnGet()
        {


        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                SuccessMesssage = "正常";
            }
            else
            {
                SuccessMesssage = "";
            }

            return Page();
        }

        private void CLearAll()
        {

        }

    }
}
