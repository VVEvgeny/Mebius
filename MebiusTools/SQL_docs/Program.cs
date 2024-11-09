using System;
using System.Text;
using System.Xml.Schema;

int logTo = 2; //1-console, 2-file
var dir = new DirectoryInfo(@"c:\_Code\meb_sql_docs");

StreamWriter sw = null;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding enc = Encoding.GetEncoding(866); 
if(logTo == 2)
{
    sw = new StreamWriter(Path.Combine(dir.FullName,"docs.sql"), false, enc);
}


var fieldsWithPossibleDefaultZaro = new HashSet<string>()
{
    "XKORA_BAL",
    "XKORA_KEY",
    "XKORA_BRN",
    "XKORA_PER",
    "XACCA_BAL",
    "XACCA_KEY",
    "XACCA_BRN",
    "XACCA_PER",
    "XKORB_BAL",
    "XKORB_KEY",
    "XKORB_BRN",
    "XKORB_PER",
    "XACCB_BAL",
    "XACCB_KEY",
    "XACCB_BRN",
    "XACCB_PER",
    "XSYM",
    "XPAYCODE",
    "XFINISH",
    "XIS9920",
    "XSECUENCE",
    "XERRORCODE",
    "XRKCKO",
    "XSALDORESULT",
    "XUSERIDEXT",
    "XBIRTH",
    "XPACKEXT",
    "XRESERVE",
    "XDOCPLACE",
    "XNUMMEMORDER",
    "XAMTPAY",
    "XREESTRCOL",
    //dturn
    "XPACK",
    "NUM_USER",
    "XDIR", //for 2d should be 1
    "XSTAT",
    "XTURNTO902",
    "XNUMPRG",
};

var defaults = new Dictionary<string,string>()
{
    //dmain
    {"XDATE","(SELECT xdate FROM xpress.daystat)"},
    {"XDOCID","NVL ((SELECT MAX (xdocid) FROM xpress.dmain WHERE xdate = (SELECT xdate FROM xpress.daystat)),0) + 1"},
    {"XDOCN", "NVL ((SELECT MAX (id) FROM xpress.uniqidtb WHERE bnk3 <> 0 AND id <= 999999),0) + 1"},//if we need diffirent AccDocNo, else we can leave "1"...
    {"XDATEB","(SELECT xdate FROM xpress.daystat)"},
    {"XKORA_CUR","'000'"},
    {"XACCA_CUR","'000'"},
    {"XKORB_CUR","'000'"},
    {"XACCB_CUR","'000'"},
    {"XDK","4"},
    {"XYEAR","1"},
    {"XPRIOR","5"},
    {"XWAY","3"},
    {"XTYPEPAY","2"},
    {"XTIME","SYSDATE"},
    {"XSPECDOC","2"},
    {"XDOCIDEXT","NVL ((SELECT MAX (id) FROM xpress.uniqidtb WHERE bnk3 <> 0 AND id <= 999999),0) + 1"},
    {"XTIMEPAY","SYSDATE"},
    {"XDOCUID","(SELECT CONCAT (CONCAT ('4583001999',TO_CHAR (xdate,'YYYYMMDD')),TO_CHAR (LPAD ( NVL ((SELECT MAX (id) FROM xpress.uniqidtb WHERE bnk3 <> 0 AND id <= 999999),0) + 1, 9,'0'))) FROM xpress.daystat)"},
    {"XSYSTEMCODE","'05'"},
    {"EDDATE","(SELECT xdate FROM xpress.daystat)"},
    {"EDAUTHOR","'4583001999'"},
    {"EDNO","(NVL ((SELECT MAX (id)FROM xpress.uniqidtb WHERE bnk3 <> 0 AND id <= 999999), 0)+ 1)"},
    {"XDISTANCE","4"},
    {"XPAYMENTPRIORITY","9"},
    {"XAMT","?"},

    //dturn
    {"XSTATDOC","1"},
    {"XACCESS","1"},
    {"XPACKUID","''"},
    {"XNR","5"},
    {"XNR_IN","5"},
    {"XTIMETURN","sysdate"},
    {"BNK_ID_BALANCE","44537002"},//GU from author
    {"BNK_ID_UBR_AUTHOR","999999999"},
    {"XNRCONS","3"},
    {"SBALANCE","44537002"}
};

var linkedFileds = new Dictionary<string,string[]>()
{
    //dmain
    {"XOPR_EXT",new [] {"XOPR"}},
    //dturn
    {"XAT_BAL",new [] {"XKORA_BAL", "XACCA_BAL"}},
    {"XAT_CUR",new [] {"XKORA_CUR", "XACCA_CUR"}},
    {"XAT_KEY",new [] {"XKORA_KEY", "XACCA_KEY"}},
    {"XAT_BRN",new [] {"XKORA_BRN", "XACCA_BRN"}},
    {"XAT_PER",new [] {"XKORA_PER", "XACCA_PER"}},

    {"XBT_BAL",new [] {"XKORB_BAL", "XACCB_BAL"}},
    {"XBT_CUR",new [] {"XKORB_CUR", "XACCB_CUR"}},
    {"XBT_KEY",new [] {"XKORB_KEY", "XACCB_KEY"}},
    {"XBT_BRN",new [] {"XKORB_BRN", "XACCB_BRN"}},
    {"XBT_PER",new [] {"XKORB_PER", "XACCB_PER"}},

    {"BNK_ID",new [] {"BNK_IDAT", "BNK_IDBT"}},
};

Dictionary<string,string> fromAcc2(string als, string acc)
{
    acc = acc.Replace(" ","");
    return new Dictionary<string, string>()
    {
        {als+"_BAL", acc.Substring(0,5)},
        {als+"_CUR","'"+acc.Substring(5,3)+"'"},
        {als+"_KEY",acc.Substring(8,1)},
        {als+"_BRN",int.Parse(acc.Substring(9,4)).ToString()},
        {als+"_PER",long.Parse(acc.Substring(13,7)).ToString()},
    };
}


var baseDoc = new Dictionary<string,string>()
    {
        //dmain
        {"XMFOA", "44537002"},
        {"XMFOB", "44537002"},

        //default {"XAMT","?"},
        {"XOPR","9"},
        {"XACCKOR","1"},

        //dturn
        {"BNK_IDAT","44537002"},
        {"BNK_IDBT","44537002"},
        {"XPRG","77"},

        //dname
        //{"xnameplat2","'test'"},

        //account
        //{"acc_id_d","990600753"},
        //{"acc_id_k","990531840"},
    }
    //.AddRange(fromAcc2("XACCA","60322 810 1 4537 0002507"))
    .AddRange(fromAcc2("XACCB","30911111111111111111"));

//baseDoc.ReplaceOrAdd(fromAcc2("XACCA","60322 810 1 4537 0002507")),

var req = new List<Dictionary<string,string>>();

var accs = new string[]
{
"60322 810 1 4537 0002507",
"60322 810 1 4537 0002468",
"60322 810 8 4537 0002454",
"60322 810 0 4537 0002429",
"60322 810 2 4537 0002410",
"60322 810 1 4537 0002138",
"60322 810 7 4537 0002130",
"60322 810 5 4537 0002084",
"60322 810 7 4537 0001940",
"60322 810 6 4537 0001875",
"60322 810 1 4537 0001812",
"60322 810 9 4537 0001685",
"60322 810 4 4537 0001635",
"60322 810 3 4537 0001544",
"60322 810 1 4537 0001511",
"60322 810 8 4537 0001497",
"60322 810 2 4537 0001440",
"60322 810 6 4537 0001435",
"60322 810 3 4537 0001298",
"60322 810 4 4537 0001237",
"60322 810 5 4537 0001234",
"60322 810 2 4537 0001220",
"60322 810 2 4537 0001152",
"60322 810 9 4537 0001151",
"60322 810 6 4537 0001095",
"60322 810 3 4537 0001094",
"60322 810 1 4537 0001087",
"60322 810 2 4537 0001084",
"60322 810 9 4537 0001054",
"60322 810 2 4537 0001026",
"60322 810 6 4537 0001011",
"60322 810 8 4537 0000922",
"60322 810 9 4537 0000783",
"60322 810 8 4537 0000702",
"60322 810 5 4537 0000691",
"60322 810 9 4537 0000699",
"60322 810 4 4537 0000665",
"60322 810 5 4537 0000646",
"60322 810 3 4537 0000600",
"60322 810 3 4537 0000529",
"60322 810 8 4537 0000524",
"60322 810 3 4537 0000480",
"60322 810 7 4537 0000323",
"60322 810 3 4537 0000147",
"60322 810 2 4537 0000140",
"60322 810 8 4537 0000090",
"60322 810 6 4537 0004623",
"60322 810 2 4537 0003794",
"60322 810 8 4537 0002946",
"60322 972 4 4537 0000011",
"60322 972 1 4537 0000010",
"60322 810 3 4537 0002721"
};

foreach(var a in accs)
{
    req.Add(new Dictionary<string, string>().AddRange(baseDoc.ReplaceOrAdd(fromAcc2("XACCA",a))));
}

var docs = new List<List<string>>();
Console.WriteLine("Docs to create:"+req.Count);

foreach(var r in req)
{
    validate(r);

    if(getValueByField(r,"XAMT") == "?")
    {
        r.ReplaceOrAdd("XAMT", 
        "(select xmtsck from xpress.account "
        +$"WHERE bnk_id = {getValueByField(r,"BNK_IDAT")} and acc_bal = {getValueByField(r,"XAT_BAL")} and acc_cur = {getValueByField(r,"XAT_CUR")} and acc_key = {getValueByField(r,"XAT_KEY")} and acc_brn = {getValueByField(r,"XAT_BRN")} and acc_per = {getValueByField(r,"XAT_PER")} "
        +")");
    }



    var dm = generateDmain(r);
    //fix xdocid
    r["XDOCID"] = "(SELECT MAX (xdocid) FROM xpress.dmain WHERE xdate = (SELECT xdate FROM xpress.daystat))";

    var dt = generateDturns(r);
    var dn = generateDname(r);
    var q = generateuniqidtb();
    var ac = updateAccounts(r);

    docs.Add(new List<string>{dm, dt, dn, q, ac});
}

int i = 1;
foreach(var d in docs)
{
    
    Console.WriteLine("-- "+i);
    sw.WriteLine("-- "+i);
    i++;
    foreach(var t in d)
    {
        Console.WriteLine(t);
        if(logTo == 2)
        {
            sw.WriteLine(t);
        }
    }
}

if(logTo == 2)
{
    sw.Flush();
    sw.Close();
    sw.Dispose();
}

void validate(Dictionary<string,string> r)
{
    if(r.ContainsKey("acc_id_d") || r.ContainsKey("acc_id_k"))
    {
        if(!r.ContainsKey("acc_id_d"))
            throw new Exception("no acc_id_d");
        if(!r.ContainsKey("acc_id_k"))
            throw new Exception("acc_id_k");
        if(r["acc_id_d"] == r["acc_id_k"])
            throw new Exception("acc_id_d == acc_id_k");

        if(getValueByField(r,"XDATE") != defaults["XDATE"])
            throw new Exception("da_opr != today we need some logic (case) to work with it in account, Date:"+getValueByField(r,"XDATE"));
    }

    if(r["XACCKOR"] != "1" && r["XACCKOR"] != "3")
        throw new Exception("XACCKOR should be 1 or 3, don't know how to work with it");
    if(r["XACCKOR"] == "1" && r["BNK_IDAT"] != r["BNK_IDBT"])
        throw new Exception("XACCKOR 1 bic a should be = bic b");
    if(r["XACCKOR"] == "3" && r["BNK_IDAT"] == r["BNK_IDBT"])
        throw new Exception("XACCKOR 3 bic a should NOT be = bic b");

    if(r["XOPR"] != "9")
        throw new Exception("XOPR should be 9");
    if(r.ContainsKey("XOPR_EXT") && r["XOPR"] != r["XOPR_EXT"])
        throw new Exception("XOPR != XOPR_EXT don't know how to work with it");
}

string getValueByField(Dictionary<string,string> r, string f)
{
    if(r.ContainsKey(f))
        return r[f];
    else if(linkedFileds.ContainsKey(f))
    {
        foreach(var l in linkedFileds[f])
        {
            if(r.ContainsKey(l))
                return r[l];
        }
    }
    else if(defaults.ContainsKey(f))
        return defaults[f];
    else if(fieldsWithPossibleDefaultZaro.Contains(f))
        return "0";

    //calculate fields


    throw new Exception("No value for field: "+f);
}

string getFieldsValues(Dictionary<string,string> r, string fields)
{
    var sF = new StringBuilder();
    var sV = new StringBuilder();

    foreach(var f in fields.Replace(" ","").Split(','))
    {
        sF.Append(f);
        sF.Append(",");

        sV.Append(getValueByField(r,f));
        sV.Append(",");
    }

    sF.Remove(sF.Length-1,1);
    sV.Remove(sV.Length-1,1);
    return "("+sF.ToString()+") VALUES ("+sV.ToString()+");";
}

string addFieldsIfExist(Dictionary<string,string> r, params string[] fields)
{
    var sb = new StringBuilder();
    foreach(var f in fields)
    {
        if(r.ContainsKey(f))
            sb.Append(","+f);
    }
    return sb.ToString();
}

string generateDmain(Dictionary<string,string> r)
{
    return $"Insert into xpress.DMAIN "
    +getFieldsValues(r, "XDATE,XDOCID,XDOCN,XDATEB,XMFOA,XKORA_BAL,XKORA_CUR,XKORA_KEY,XKORA_BRN,XKORA_PER,XACCA_BAL,XACCA_CUR,XACCA_KEY,XACCA_BRN,XACCA_PER,XMFOB,XKORB_BAL,XKORB_CUR,XKORB_KEY,XKORB_BRN,XKORB_PER,XACCB_BAL,XACCB_CUR,XACCB_KEY,XACCB_BRN,XACCB_PER,XAMT,XDK,XOPR,XSYM,XPAYCODE,XACCKOR,XFINISH,XYEAR,XPRIOR,XWAY,XTYPEPAY,XTIME,XSPECDOC,XIS9920,XSECUENCE,XDOCIDEXT,XERRORCODE,XRKCKO,XSALDORESULT,XUSERIDEXT,XBIRTH,XPACKEXT,XTIMEPAY,XDOCUID,XRESERVE,XDOCPLACE,XNUMMEMORDER,XSYSTEMCODE,EDDATE,EDAUTHOR,EDNO,XAMTPAY,XREESTRCOL,XOPR_EXT,XDISTANCE,XPAYMENTPRIORITY");
}

string generateDturns(Dictionary<string,string> r)
{
    if(r["BNK_IDAT"] != r["BNK_IDBT"])
    {
        var r1 = new Dictionary<string,string>().AddRange(r);
        r1.ReplaceOrAdd("BNK_IDBT","0");
        r1.ReplaceOrAdd("XBT_BAL","0");
        r1.ReplaceOrAdd("XBT_CUR","'000'");
        r1.ReplaceOrAdd("XBT_KEY","0");
        r1.ReplaceOrAdd("XBT_BRN","0");
        r1.ReplaceOrAdd("XBT_PER","0");
        r1.ReplaceOrAdd("BNK_ID",r1["BNK_IDAT"]);

        var r2 = new Dictionary<string,string>().AddRange(r);
        r2.ReplaceOrAdd("BNK_IDAT","0");
        r2.ReplaceOrAdd("XAT_BAL","0");
        r2.ReplaceOrAdd("XAT_CUR","'000'");
        r2.ReplaceOrAdd("XAT_KEY","0");
        r2.ReplaceOrAdd("XAT_BRN","0");
        r2.ReplaceOrAdd("XAT_PER","0");
        r2.ReplaceOrAdd("BNK_ID_UBR_AUTHOR","0");
        r2.ReplaceOrAdd("XDIR","1");
        r1.ReplaceOrAdd("BNK_ID",r1["BNK_IDBT"]);

        var t1 =
        $"Insert into xpress.DTURN "
        +getFieldsValues(r1, "XDATE, XDOCID, BNK_ID, XPACK, BNK_IDAT, XAT_BAL, XAT_CUR, XAT_KEY, XAT_BRN, XAT_PER, BNK_IDBT, XBT_BAL, XBT_CUR, XBT_KEY, XBT_BRN, XBT_PER, NUM_USER, XDIR, XSTAT, XSTATDOC, XACCESS, XPRG, XTURNTO902, XPACKUID, XNR, XTIMETURN,XNR_IN, BNK_ID_BALANCE, BNK_ID_UBR_AUTHOR, XNUMPRG, XNRCONS,  SBALANCE");

        var t2 =
        $"Insert into xpress.DTURN "
        +getFieldsValues(r2, "XDATE, XDOCID, BNK_ID, XPACK, BNK_IDAT, XAT_BAL, XAT_CUR, XAT_KEY, XAT_BRN, XAT_PER, BNK_IDBT, XBT_BAL, XBT_CUR, XBT_KEY, XBT_BRN, XBT_PER, NUM_USER, XDIR, XSTAT, XSTATDOC, XACCESS, XPRG, XTURNTO902, XPACKUID, XNR, XTIMETURN,XNR_IN, BNK_ID_BALANCE, BNK_ID_UBR_AUTHOR, XNUMPRG, XNRCONS,  SBALANCE");


        return t1+Environment.NewLine+t2;
//Insert into xpress.DTURN (XDATE, XDOCID,  
//BNK_ID, XPACK, BNK_IDAT, XAT_BAL, XAT_CUR, XAT_KEY, XAT_BRN, XAT_PER,  BNK_IDBT, XBT_BAL, XBT_CUR, XBT_KEY, XBT_BRN, XBT_PER, NUM_USER, XDIR, XSTAT, XSTATDOC, XACCESS, XPRG, XTURNTO902, XPACKUID, XNR, XTIMETURN,XNR_IN, BNK_ID_BALANCE, BNK_ID_UBR_AUTHOR, XNUMPRG, XNRCONS,  SBALANCE) 
//Values ((select xdate from xpress.daystat), (select MAX(xdocid) from xpress.dmain where xdate = (select xdate from xpress.daystat)), 
//44537002, 0, 44537002, 70101, '810', 0, 4537, 118003, 0, 0, '000', 0, 0, 0, 0, 0, 0, 1, 1, 77, 0, '', 5, sysdate, 5, 44537002, 999999999, 0, 3, 44537002);

//Insert into xpress.DTURN (XDATE, XDOCID,  
//BNK_ID, XPACK, BNK_IDAT, XAT_BAL, XAT_CUR, XAT_KEY, XAT_BRN, XAT_PER,  BNK_IDBT, XBT_BAL, XBT_CUR, XBT_KEY, XBT_BRN, XBT_PER, NUM_USER, XDIR, XSTAT, XSTATDOC, XACCESS, XPRG, XTURNTO902, XPACKUID, XNR, XTIMETURN,XNR_IN, BNK_ID_BALANCE, BNK_ID_UBR_AUTHOR, XNUMPRG, XNRCONS,  SBALANCE) 
//Values ((select xdate from xpress.daystat), (select MAX(xdocid) from xpress.dmain where xdate = (select xdate from xpress.daystat)), 
//44501002, 0, 0, 0, '000', 0, 0, 0, 44501002, 47427, '810', 9, 4501, 4019234, 0, 0, 0, 1, 1, 77, 0, '', 5, sysdate, 5, 44537002, 0, 0, 3, 44537002);

    }
    else 
        return $"Insert into xpress.DTURN "
        +getFieldsValues(r, "XDATE, XDOCID, BNK_ID, XPACK, BNK_IDAT, XAT_BAL, XAT_CUR, XAT_KEY, XAT_BRN, XAT_PER, BNK_IDBT, XBT_BAL, XBT_CUR, XBT_KEY, XBT_BRN, XBT_PER, NUM_USER, XDIR, XSTAT, XSTATDOC, XACCESS, XPRG, XTURNTO902, XPACKUID, XNR, XTIMETURN,XNR_IN, BNK_ID_BALANCE, BNK_ID_UBR_AUTHOR, XNUMPRG, XNRCONS,  SBALANCE");
}

string generateDname(Dictionary<string,string> r)
{
    return $"Insert into xpress.DNAME "
    + getFieldsValues(r, "XDATE,XDOCID" + addFieldsIfExist(r,"xnameplat1","xnameplat2","xnameplat3","xnameplat4","xnameplat5","xnameplat6","xnameplat7","xnameplat8")
    );
}

string generateuniqidtb()
{
    return "insert into xpress.uniqidtb (bnk7,bnk3,id) Values (0,1,( NVL ((SELECT MAX (id) FROM xpress.uniqidtb WHERE bnk3 <> 0 AND id <= 999999), 0) + 1));";
}

string updateAccounts(Dictionary<string,string> r)
{
    var sb = new StringBuilder();
    if(r.ContainsKey("acc_id_d") && r.ContainsKey("acc_id_k"))
    {    
        sb.AppendLine("-- deb");
        sb.AppendLine($"UPDATE xpress.account SET rest = rest - {getValueByField(r,"XAMT")} , xMTsck = xMTsck - {getValueByField(r,"XAMT")} , xMDebet = xMDebet + {getValueByField(r,"XAMT")} , doc_ecnt = doc_ecnt + 1, da_opr = ( SELECT xdate FROM xpress.daystat ), xLCntDebet = xLCntDebet + 1, xLCntDebetVPS = xLCntDebetVPS + 1 WHERE acc_id = {getValueByField(r,"acc_id_d")} ;");

        sb.AppendLine("-- cre");
        sb.AppendLine($"UPDATE xpress.account SET rest = rest + {getValueByField(r,"XAMT")} , xMTsck = xMTsck + {getValueByField(r,"XAMT")} , xMCredit = xMCredit + {getValueByField(r,"XAMT")} , doc_ecnt = doc_ecnt + 1, da_opr = ( SELECT xdate FROM xpress.daystat ), xLCntCredit = xLCntCredit + 1, xLCntCreditVPS = xLCntCreditVPS + 1 WHERE acc_id = {getValueByField(r,"acc_id_k")} ;");
    }
    else
    {
        //this need to be credit first if we get xamt from account
        sb.AppendLine("-- cre");
        sb.AppendLine($"UPDATE xpress.account SET rest = rest + {getValueByField(r,"XAMT")} , xMTsck = xMTsck + {getValueByField(r,"XAMT")} , xMCredit = xMCredit + {getValueByField(r,"XAMT")} , doc_ecnt = doc_ecnt + 1, da_opr = ( SELECT xdate FROM xpress.daystat ), xLCntCredit = xLCntCredit + 1, xLCntCreditVPS = xLCntCreditVPS + 1 "
        +$"WHERE bnk_id = {getValueByField(r,"BNK_IDBT")} and acc_bal = {getValueByField(r,"XBT_BAL")} and acc_cur = {getValueByField(r,"XBT_CUR")} and acc_key = {getValueByField(r,"XBT_KEY")} and acc_brn = {getValueByField(r,"XBT_BRN")} and acc_per = {getValueByField(r,"XBT_PER")} ;"
        );

        sb.AppendLine("-- deb");
        sb.AppendLine($"UPDATE xpress.account SET rest = rest - {getValueByField(r,"XAMT")} , xMTsck = xMTsck - {getValueByField(r,"XAMT")} , xMDebet = xMDebet + {getValueByField(r,"XAMT")} , doc_ecnt = doc_ecnt + 1, da_opr = ( SELECT xdate FROM xpress.daystat ), xLCntDebet = xLCntDebet + 1, xLCntDebetVPS = xLCntDebetVPS + 1 "
        +$"WHERE bnk_id = {getValueByField(r,"BNK_IDAT")} and acc_bal = {getValueByField(r,"XAT_BAL")} and acc_cur = {getValueByField(r,"XAT_CUR")} and acc_key = {getValueByField(r,"XAT_KEY")} and acc_brn = {getValueByField(r,"XAT_BRN")} and acc_per = {getValueByField(r,"XAT_PER")} ;"
        );
    }
    return sb.ToString();
}






public static class Ext
{
    public static Dictionary<T, S> ReplaceOrAdd<T, S>(this Dictionary<T, S> source, T key, S value)
    {
                if(!source.ContainsKey(key))
                    source.Add(key, value);
                else
                    source[key] = value;
                return source;
    }

    public static Dictionary<T, S> ReplaceOrAdd<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
    {
        return source.AddRange(collection,true);
    }

    public static Dictionary<T, S> AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection, bool replaceIfExist = false)
    {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if(!source.ContainsKey(item.Key)){ 
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                    if(replaceIfExist)
                        source[item.Key] = item.Value;
                }  
            } 
            return source;
    }
}