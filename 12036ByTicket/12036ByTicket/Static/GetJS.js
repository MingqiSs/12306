function hashAlg (a, b, c) {
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
    c = a.length;
    d = "";
    a = d = 0 == a.length % 2 ? a.substring(c / 2, c) + a.substring(0, c / 2) : a.substring(c / 2 + 1, c) + a.charAt(c / 2) + a.substring(0, c / 2);
    c = "";
    for (d = a.length - 1; 0 <= d; d--)
        c += a.charAt(d);
    a = Ra(c);
    //a = R.SHA256(a).toString(R.enc.Base64);
    //c = Ra(a);
    //c = R.SHA256(c).toString(R.enc.Base64);
    return b+"|"+a
  //  return c
}
function n(a, b) {
    this.key = a;
    this.value = b
}
function Ra(a) {
    var b = a.length
        , c = 0 == b % 3 ? parseInt(b / 3) : parseInt(b / 3) + 1;
    if (3 > b)
        return a;
    var d = a.substring(0, 1 * c)
        , e = a.substring(1 * c, 2 * c);
    return a.substring(2 * c, b) + d + e
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
   // cookieCode: "VySQ",
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
