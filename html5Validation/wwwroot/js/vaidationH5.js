// クライアント側で「input」タグのバリデーションメッセージを変更するために
// 「invalid」イベントでメッセージを設定するイベントハンドラーを設定
window.onload = function () {

    // inputタグのエレメントをすべて取得
    var inputList = document.getElementsByTagName('input');

    for (var i = 0; i < inputList.length; i++) {

        // ブラウザがサポートしないバリデーションをサポートさせるためにinputイベントで独自バリデーションを行う。
        // ただし、invalidでCustomErrorを利用してメッセージを変更しているため、入力が変更されたときにカスタムエラーをクリアしておく必要があるので
        // 全てのinputタグの入力時にカスタムエラーをクリアしている（カスタムエラーを利用しうるものだけでいいのだが、判定が複雑になるので全部処理）
        inputList[i].addEventListener('input', function (e) {

            // ブラウザが判定したエラーが有ればカスタムエラーを削除して終了
            // ブラウザサポートのエラーが判定しているので独自のエラー判定は行わない
            // バリデーションの種類が増えたら条件を追加する伊必要がある
            if (e.target.validity.badInput ||
                e.target.validity.patternMismatch ||
                e.target.validity.rangeOverflow ||
                e.target.validity.rangeUnderflow ||
                e.target.validity.stepMismatch ||
                e.target.validity.tooLong ||
                e.target.validity.tooShort ||
                e.target.validity.typeMismatch ||
                e.target.badInput) {

                // ここでカスタムエラーを削除
                e.target.setCustomValidity("");

                return;
            }

            // Requiredバリデーション
            // これに対応していないバリデーションは無いはずなので、このブロックはなくてもいいはず
            if (e.target.value === null || e.target.value.length === 0)
            {
                errMsg = e.target.getAttribute("required-err-msg");
                if (errMsg === null) {
                    e.target.setCustomValidity(e.target.getAttribute("notsupported-required-err-msg"));
                }
                else {
                    e.target.setCustomValidity(errMsg);
                }
            }

            // StringLengthバリデーション
            // tooLongは全て対応してるみたいですが（そもそもMaxLength以上入力できない）
            // エラーが無いのでカスタムバリデーションエラーをクリア
            maxLength = e.target.getAttribute("maxlength");
            if (getNumType(maxLength) === 0) {
                if (e.target.value.length > maxLength) {
                    errMsg = e.target.getAttribute("max-length-err-msg");
                    if (errMsg === null) {
                        e.target.setCustomValidity(e.target.getAttribute("notsupported-max-length-err-msg"));
                        return;
                    }
                    else {
                        e.target.setCustomValidity(errMsg);
                        return;
                    }
                }
            }
            minlength = e.target.getAttribute("minlength");
            if (getNumType(minlength) === 0 && e.target.value.length > 0) {
                if (e.target.value.length < minlength) {
                    errMsg = e.target.getAttribute("min-length-err-msg");
                    if (errMsg === null) {
                        e.target.setCustomValidity(e.target.getAttribute("notsupported-min-length-err-msg"));
                        return;
                    }
                    else {
                        e.target.setCustomValidity(errMsg);
                        return;
                    }
                }
            }


            // Compareバリデーション
            var compareErrorElement = null;
            var compareElement = e.target.getAttribute("compare-item-id");
            if (compareElement !== null) {
                // 自身がCompareのエレメントの場合の対応
                if (e.target.value !== null || e.target.value !== "") {
                    var elements = document.getElementsByTagName('input');
                    for (var j = 0; j < elements.length; j++) {
                        if (elements[j].id === e.target.getAttribute("compare-item-id")) {
                            if (elements[j].value !== e.target.value) {
                                e.target.setCustomValidity(e.target.getAttribute("compare-err-msg"));
                                return;
                            }
                        }
                    }
                }
            }
            else {
                // 自身がCompareの対象となっているエレメントの対応
                var elements2 = document.getElementsByTagName('input');
                for (var k = 0; k < elements2.length; k++) {
                    if (elements2[k].getAttribute("compare-item-id") === e.target.id) {
                        if (elements2[k].value !== e.target.value) {
                            elements2[k].setCustomValidity(elements2[k].getAttribute("compare-err-msg"));
                        }
                        else {
                            elements2[k].setCustomValidity("");
                        }
                    }
                }
            }

            if (e.target.value !== null && e.target.value.length > 0) {
                // 列挙バリデーション
                var enumValuesStr = e.target.getAttribute("enum-values");
                if (enumValuesStr !== null) {
                    isEnumError = true;
                    var enumVals = e.target.getAttribute("enum-values").split(",");
                    for (var i = 0; i < enumVals.length; i++) {
                        if (e.target.value === enumVals[i]) {
                            isEnumError = false;
                            break;
                        }
                    }
                    if (isEnumError) {
                        e.target.setCustomValidity(e.target.getAttribute("enum-err-msg"));
                        return;
                    }
                }

                // Dateバリデーション
                var typeAttribute = e.target.getAttribute("type");
                if (typeAttribute === "date") {
                    if (!isDate(e.target.value)) {
                        e.target.setCustomValidity(e.target.getAttribute("date-err-msg"));
                        return;
                    }
                }

                // メールアドレスバリデーション
                if (typeAttribute === "email") {
                    if (!isEmail(e.target.value)) {
                        emailErrMsg = e.target.getAttribute("typemis-err-msg");
                        if (emailErrMsg === null) {
                            e.target.setCustomValidity(e.target.getAttribute("email-err-msg"));
                        }
                        else {
                            e.target.setCustomValidity(emailErrMsg);
                        }
                        return;
                    }
                }

                // 正規表現バリデーション（対象のブラウザでは全てサポートされているので実行できない）
                var patternAttribute = e.target.getAttribute("pattern");
                if (patternAttribute !== null) {
                    var muchRes = e.target.value.match(patternAttribute);
                    if (muchRes === null || muchRes[0] !== patternAttribute) {
                        patternErrorMsg = e.target.getAttribute("regular-err-msg");
                        if (patternErrorMsg === null) {
                            e.target.setCustomValidity(e.target.getAttribute("notsupported-regular-err-msg"));
                        }
                        else {
                            e.target.setCustomValidity(patternErrorMsg);
                        }
                        return;
                    }
                }

                // Rangeバリデーション
                maxVal = e.target.getAttribute("max");
                minVal = e.target.getAttribute("min");
                numericType = e.target.getAttribute("numeric-type");
                if (maxVal !== null || minVal !== null) {
                    if (numericType === "Integer") {

                        if (maxVal !== null) {

                            if (parseInt(e.target.value) > parseInt(maxVal)) {
                                if (e.target.getAttribute("max-err-msg")) {
                                    e.target.setCustomValidity(e.target.getAttribute("max-err-msg"));
                                    return;
                                }
                                else {
                                    e.target.setCustomValidity(e.target.getAttribute("notsupported-max-err-msg"));
                                    return;
                                }
                            }
                        }

                        if (minVal !== null) {
                            if (parseInt(e.target.value) < parseInt(minVal)) {
                                if (e.target.getAttribute("min-err-msg")) {
                                    e.target.setCustomValidity(e.target.getAttribute("min-err-msg"));
                                    return;
                                }
                                else {
                                    e.target.setCustomValidity(e.target.getAttribute("notsupported-min-err-msg"));
                                    return;
                                }
                            }
                            if (step !== null) {
                                if ((parseInt(e.target.value) - parseInt(minVal)) % parseInt(step) !== 0) {

                                    if (e.target.getAttribute("step-err-msg")) {
                                        e.target.setCustomValidity(e.target.getAttribute("step-err-msg"));
                                        return;
                                    }
                                    else {
                                        e.target.setCustomValidity(e.target.getAttribute("notsupported-step-err-msg"));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (!isNumber(e.target.value)) {
                            e.target.setCustomValidity("実数を入力してください。");
                            return;
                        }

                        if (maxVal !== null) {
                            if (parseFloat(e.target.value) > parseFloat(maxVal)) {
                                if (e.target.getAttribute("max-err-msg")) {
                                    e.target.setCustomValidity(e.target.getAttribute("max-err-msg"));
                                    return;
                                }
                                else {
                                    e.target.setCustomValidity(e.target.getAttribute("notsupported-max-err-msg"));
                                    return;
                                }
                            }
                        }

                        if (minVal !== null) {
                            if (parseFloat(e.target.value) < parseFloat(minVal)) {
                                if (e.target.getAttribute("min-err-msg")) {
                                    e.target.setCustomValidity(e.target.getAttribute("min-err-msg"));
                                    return;
                                }
                                else {
                                    e.target.setCustomValidity(e.target.getAttribute("notsupported-min-err-msg"));
                                    return;
                                }
                            }
                            step = e.target.getAttribute("step");
                            if (step !== null) {
                                under1MaxLength = GetUnder1MaxLength(0, minVal);
                                under1MaxLength = GetUnder1MaxLength(under1MaxLength, e.target.value);
                                under1MaxLength = GetUnder1MaxLength(under1MaxLength, step);
                                rate = 1;
                                for (i = 0; i < under1MaxLength; i++) rate = rate * 10;
                                minInt = Math.round(minVal * rate);
                                stepInt = Math.round(step * rate);
                                valInt = Math.round(e.target.value * rate);
                                if ((valInt - minInt) % stepInt !== 0) {
                                    if (e.target.getAttribute("step-err-msg")) {
                                        e.target.setCustomValidity(e.target.getAttribute("step-err-msg"));
                                        return;
                                    }
                                    else {
                                        e.target.setCustomValidity(e.target.getAttribute("notsupported-step-err-msg"));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            e.target.setCustomValidity("");
        });


        // 全てのinputエレメントの「invalid」イベントにバリデーションチェックエラーの場合のエラーメッセージを設定
        inputList[i].addEventListener('invalid', function (e) {

            if (e.target.validity.valueMissing) {
                var edittedValidationMessage = e.target.getAttribute("required-err-msg");
                if (edittedValidationMessage !== null && edittedValidationMessage.length > 0) {
                    e.target.setCustomValidity(edittedValidationMessage);
                }
            }
            else if (e.target.validity.typeMismatch) {
                var typemismatchErroMsg = e.target.getAttribute("typemis-err-msg");
                if (typemismatchErroMsg !== null && typemismatchErroMsg.length > 0) {
                    e.target.setCustomValidity(typemismatchErroMsg);
                }
            }
            else if (e.target.validity.tooLong){
                var maxLengthErrorMsg = e.target.getAttribute("max-length-err-msg");
                if (maxLengthErrorMsg !== null && maxLengthErrorMsg.length > 0) {
                    e.target.setCustomValidity(maxLengthErrorMsg);
                }
            }
            else if (e.target.validity.tooShort) {
                var minLengthErrorMsg = e.target.getAttribute("min-length-err-msg");
                if (minLengthErrorMsg !== null && minLengthErrorMsg.length > 0) {
                    e.target.setCustomValidity(minLengthErrorMsg);
                }
            }
            else if (e.target.validity.patternMismatch) {
                var patternMismatchErrorMsg = e.target.getAttribute("regular-err-msg");
                if (patternMismatchErrorMsg !== null && patternMismatchErrorMsg.length > 0) {
                    e.target.setCustomValidity(patternMismatchErrorMsg);
                }
            }
            else if (e.target.validity.rangeOverflow) {
                var rangeOverflowErrorMsg = e.target.getAttribute("max-err-msg");
                if (rangeOverflowErrorMsg !== null && rangeOverflowErrorMsg.length > 0) {
                    e.target.setCustomValidity(rangeOverflowErrorMsg);
                }
            }
            else if (e.target.validity.rangeUnderflow) {
                var rangeUnderflowErrorMsg = e.target.getAttribute("min-err-msg");
                if (rangeUnderflowErrorMsg !== null && rangeUnderflowErrorMsg.length > 0) {
                    e.target.setCustomValidity(rangeUnderflowErrorMsg);
                }
            }
            else if (e.target.validity.stepMismatch) {
                var stepErrorMsg = e.target.getAttribute("step-err-msg");
                if (stepErrorMsg !== null && stepErrorMsg.length > 0) {
                    e.target.setCustomValidity(stepErrorMsg);
                }
            }
            else {
                if (!e.target.validity.customError) {
                    e.target.setCustomValidity("");
                }
            }

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

function isNumber(x) {
    if (typeof (x) !== 'number' && typeof (x) !== 'string')
        return false;
    else
        return (x == parseFloat(x) && isFinite(x));
}

function isDate(strDate) {
    // 空文字は無視
    if (strDate === "") {
        return true;
    }
    // 年/月/日の形式のみ許容する
    if (!strDate.match(/^\d{4}\/\d{1,2}\/\d{1,2}$/)) {
        return false;
    }

    // 日付変換された日付が入力値と同じ事を確認
    // new Date()の引数に不正な日付が入力された場合、相当する日付に変換されてしまうため
    // 
    var date = new Date(strDate);
    if (date.getFullYear() !== Number(strDate.split("/")[0])
        || date.getMonth() !== Number(strDate.split("/")[1] - 1)
        || date.getDate() !== Number(strDate.split("/")[2])
    ) {
        return false;
    }

    return true;
}

function isEmail(mailAddress) {

    // 空文字は無視
    if (mailAddress === "") {
        return true;
    }

    // 年/月/日の形式のみ許容する
    return mailAddress.match(/^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);
}

// 数値タイプの判定
// -1:エラー
//  0:整数
//  1:実数
function getNumType(strValue) {

    if (strValue === null) return -1;
    if (strValue.length === 0) return -1;

    // 小数点が有れば整数ではない
    firstComma = strValue.indexOf('.');
    if (firstComma >= 0) {
        nextComma = strValue.indexOf('.', firstComma + 1);
        if (nextComma > 0) {
            return -1;    // コンマが2つ以上あれば数値ではない
        }
        else {
            // 小数点を破棄して整数チェック
            dropCommaStr = strValue.replace('.', '');
            if (isNaN(dropCommaStr)) return -1;     // コンマ無しで数値ではないので数値ではない

            // コンマが一つで数値なので実数
            return 1;
        }
    }
    else {
        if (isNaN(strValue)) {
            // 数値でなければ
            return -1;
        } else {
            // 小数点が無く数値なので整数
            return 0;
        }
    }
}

function GetUnder1MaxLength(beforeMaxLength, numberStr) {

    numberStrDivided = numberStr.split('.');

    if (numberStrDivided.length > 1) {
        under1Str = numberStrDivided[1];
        while (under1Str[under1Str.length - 1] === '0') {
            if (under1Str.length === 1) {
                under1Str = "";
                break;
            }
            under1Str = under1Str.substr(0, under1Str.length - 1);
        }
        if (beforeMaxLength < under1Str.length) {
            return under1Str.length;
        }
        else {
            return beforeMaxLength;
        }
    }
    else {
        return beforeMaxLength;
    }
}
