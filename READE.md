
# RSSReader (C#)
C#でRSSを取得するクラス. HttpClientを使ってGetAsyncしている.
GoogleAlertReaderはGoogleAlertからHTMLをGETしてXMLをパースする.

## 使い方 : GoogleAlertReader

```cs

Task<GoogleAlertBody> task = reader.ReadAsyncAndParse(url_googlealert);

task.Wait();

GoogleAlertBody gabody = task.Result;

// GoogleAlertBodyのエラーチェック
Assert.AreNotEqual(gabody.title, "");
Assert.AreNotEqual(gabody.link, "");

Assert.IsTrue(gabody.Entries.Count > 0);

```


