using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using html5Validation.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace html5Validation.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public TestInputs Inputs { get; set; }

        public class TestInputs
        {
            [RequiredH5]
            public string RequireString { get; set; }

            [StringLengthH5(10, 5)]
            public string StringLengthString { get; set; }
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
