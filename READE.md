
# RSSReader (C#)
C#��RSS���擾����N���X. HttpClient���g����GetAsync���Ă���.
GoogleAlertReader��GoogleAlert����HTML��GET����XML���p�[�X����.

## �g���� : GoogleAlertReader

```cs

Task<GoogleAlertBody> task = reader.ReadAsyncAndParse(url_googlealert);

task.Wait();

GoogleAlertBody gabody = task.Result;

// GoogleAlertBody�̃G���[�`�F�b�N
Assert.AreNotEqual(gabody.title, "");
Assert.AreNotEqual(gabody.link, "");

Assert.IsTrue(gabody.Entries.Count > 0);

```


