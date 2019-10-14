function hashAlg(a, b, c) {  
    a.sort(function (a, b) {
        var c, d;
        if ("object" === typeof a && "object" === typeof b && a && b)
            return c = a.key,
                d = b.key,
                c === d ? 0 : typeof c === typeof d ? c < d ? -1 : 1 : typeof c < typeof d ? -1 : 1;
        throw "error";
    });
    for (var d = 0; d < a.length; d++) {
        var e = a[d].key.replace(RegExp("%", "gm"), "")
            , f = ""
            , f = "string" == typeof a[d].value ? a[d].value.replace(RegExp("%", "gm"), "") : a[d].value;
        "" !== f && (c += e + f,
            b += "\x26" + (void 0 == ib[e] ? e : ib[e]) + "\x3d" + f)
    }
    a = c;
    c = "";
    d = a.length;
    for (e = 0; e < d; e++)
        f = a.charAt(e).charCodeAt(0),
            c = 127 === f ? c + String.fromCharCode(0) : c + String.fromCharCode(f + 1);
    a = c;
    c = a.length;
    d = a.split("");
    for (e = 0; e < parseInt(c / 2); e++)
        0 == e % 2 && (f = a.charAt(e),
            d[e] = d[c - 1 - e],
            d[c - 1 - e] = f);
    a = d.join("");
    a = Ra(a);
    return b + "|" + a
}
function n(a, b) {
    this.key = a;
    this.value = b
}
function Ra(a) {
    for (var b = "", c = a.length - 1; 0 <= c; c--)
        b += a.charAt(c);
    return b
}
var ib = {
    srcScreenSize: "tOHY",
    cookieEnabled: "VPIf",
    openDatabase: "V8vl",
    historyList: "kU5z",
    javaEnabled: "yD16",
    hasLiedResolution: "3neK",
    plugins: "ks0Q",
    hasLiedOs: "ci5c",
    browserName: "-UVA",
    hasLiedLanguages: "j5po",
    userLanguage: "hLzX",
    storeDb: "Fvje",
    cookieCode: "VySQ",
    doNotTrack: "VEek",
    flashVersion: "dzuS",
    browserLanguage: "q4f3",
    mimeTypes: "jp76",
    localCode: "lEnu",
    userAgent: "0aew",
    timeZone: "q5aJ",
    scrWidth: "ssI5",
    hasLiedBrowser: "2xC5",
    webSmartID: "E3gR",
    adblock: "FMQw",
    scrAvailHeight: "88tV",
    indexedDb: "3sw-",
    systemLanguage: "e6OK",
    scrHeight: "5Jwy",
    scrAvailSize: "TeRS",
    scrDeviceXDPI: "3jCe",
    appMinorVersion: "qBVW",
    localStorage: "XM7l",
    sessionStorage: "HVia",
    online: "9vyE",
    scrAvailWidth: "E-lJ",
    cpuClass: "Md7A",
    touchSupport: "wNLf",
    os: "hAqN",
    appcodeName: "qT7b",
    scrColorDepth: "qmyu",
    browserVersion: "d435",
    jsFonts: "EOQP"
};
