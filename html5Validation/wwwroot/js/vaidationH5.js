// クライアント側で「input」タグのバリデーションメッセージを変更するために
// 「invalid」イベントでメッセージを設定するイベントハンドラーを設定
window.onload = function () {

    // inputタグのエレメントをすべて取得
    var inputList = document.getElementsByTagName('input');

    for (var i = 0; i < inputList.length; i++) {

        // Compaaireバリデーションのためにinput時のに入力比較バリデーション処理を追加
        var compareElement = inputList[i].getAttribute("compare-item-id");
        if (compareElement !== null) {

            inputList[i].addEventListener('input', function (e) {

                var elements = document.getElementsByTagName('input');
                for (var j = 0; j < elements.length; j++) {
                    if (elements[j].id === e.target.getAttribute("compare-item-id")) {
                        if (elements[j].value !== e.target.value) {
                            e.target.setCustomValidity(e.target.getAttribute("compare-err-msg"));
                        }
                        else {
                            e.target.setCustomValidity("");
                        }
                    }
                }
            });
        }

        var enumValuesStr = inputList[i].getAttribute("enum-values");
        if (enumValuesStr !== null) {

            inputList[i].addEventListener('input', function (e) {

                var enumVals = e.target.getAttribute("enum-values").split(",");
                for (var i = 0; i < enumVals.length; i++) {
                    if (e.target.value === enumVals[i]) {
                        e.target.setCustomValidity("");
                        return;
                    }
                }
                e.target.setCustomValidity(e.target.getAttribute("enum-err-msg"));

            });
        }


        // 全てのinputエレメントの「invalid」イベントにバリデーションチェックエラーの場合のエラーメッセージを設定
        inputList[i].addEventListener('invalid', function (e) {

            if (e.target.validity.valueMissing) {
                var edittedValidationMessage = e.target.getAttribute("required-err-msg");
                if ((edittedValidationMessage === null) || (edittedValidationMessage.length <= 0)) {
                    edittedValidationMessage = e.target.validationMessage;
                }

                e.target.setCustomValidity(edittedValidationMessage);
            }
            else if (e.target.validity.typeMismatch) {
                var typemismatchErroMsg = e.target.getAttribute("typemis-err-msg");
                if ((typemismatchErroMsg === null) || (typemismatchErroMsg.length <= 0)) {
                    typemismatchErroMsg = e.target.validationMessage;
                }

                e.target.setCustomValidity(typemismatchErroMsg);
            }
            else if ((e.target.validity.tooLong) || (e.target.validity.toShort)){
                var strlenErrorMsg = e.target.getAttribute("stringLength-err-msg");
                if (strlenErrorMsg.length <= 0) {
                    strlenErrorMsg = e.target.validationMessage;
                }

                e.target.setCustomValidity(tooLongErrorMsg);
            }
            else if (e.target.validity.toShort) {
                var tooShortErrorMsg = e.target.getAttribute("stringLength-err-msg");
                if (tooShortErrorMsg.length <= 0) {
                    tooShortErrorMsg = e.target.validationMessage;
                }

                e.target.setCustomValidity(tooShortErrorMsg);
            }
            else if (e.target.validity.patternMismatch) {
                var patternMismatchErrorMsg = e.target.getAttribute("regular-err-msg");
                if (patternMismatchErrorMsg.length <= 0) {
                    patternMismatchErrorMsg = e.target.validationMessage;
                }

                e.target.setCustomValidity(patternMismatchErrorMsg);
            }
            else {
                e.target.setCustomValidity("");
            }
            // elseの前にエラー情報に対するエラーメッセージを追加する予定

        }, false);
    }

    // 全てのバリデーションサマリーのエレメントを取得
    var summaryErrorElements = document.getElementsByClassName('validation-summary-valid');

    for (i = 0; i < summaryErrorElements.length; i++) {

        // サマリー内はリストになっていて、「display=none」しかなければサマリーは非表示、以外のものが有ればサマリーを表示。
        var summaryElements = summaryErrorElements[i].getElementsByTagName('li');

        var existFlag = false;

        for (var j = 0; j < summaryElements.length; j++) {
            if (summaryElements[j].style.display !== 'none') {
                summaryErrorElements[i].style.display = 'block';
                existFlag = true;
            }
        }
        if (existFlag === false) summaryErrorElements[i].style.display = 'none';
    }
};


