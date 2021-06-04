using NUnit.Framework;

namespace com.amabie.EngrossingTime
{
    public class EngrossingTimeTest
    {
        public EngrossingTime CreateInstance()
        {
            return new EngrossingTime();
        }

        [Test]
        public void SetFeltTimeTest()
        {
            // 共通テスト引数1
            var i = CreateInstance();
            i.SetFeltTerm(10f);
            Assert.AreEqual(0f, i.FeltTerm.BeginningTime);
            Assert.AreEqual(10f, i.FeltTerm.EndingTime);

            // 共通テスト引数2
            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            Assert.AreEqual(20f, i.FeltTerm.BeginningTime);
            Assert.AreEqual(30f, i.FeltTerm.EndingTime);

            // 浮動小数点の場合
            i = CreateInstance();
            i.SetFeltTerm(40.12f, 500.34f);
            Assert.AreEqual(40.12f, i.FeltTerm.BeginningTime);
            Assert.AreEqual(500.34f, i.FeltTerm.EndingTime);

            // 負の値
            // 想定外だが、機能的には時間が減少する UX も可能
            // 下限もチェック
            i = CreateInstance();
            i.SetFeltTerm(100f, 50f);
            Assert.AreEqual(100f, i.FeltTerm.BeginningTime);
            Assert.AreEqual(50f, i.FeltTerm.EndingTime);
        }

        [Test]
        public void UpdateFeltTime()
        {
            // 共通テスト引数1
            var i = CreateInstance();
            i.SetFeltTerm(10f);
            Assert.AreEqual(0f, i.DisplayFeltTime);
            i.UpdateFeltTime(1.23f);
            Assert.AreEqual(1.2f, i.DisplayFeltTime);

            // 共通テスト引数2
            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            Assert.AreEqual(20f, i.DisplayFeltTime);
            i.UpdateFeltTime(1.23f);
            Assert.AreEqual(21.2f, i.DisplayFeltTime);

            // 上限チェック
            i = CreateInstance();
            i.SetFeltTerm(40f, 500f);
            Assert.AreEqual(40f, i.DisplayFeltTime);
            i.UpdateFeltTime(1000f);
            Assert.AreEqual(500f, i.DisplayFeltTime);

            // 負の値
            // 想定外だが、機能的には時間が減少する UX も可能
            // 下限もチェック
            // 減少する場合は第二引数は 0 を基本想定している
            i = CreateInstance();
            i.SetFeltTerm(100f, 0f);
            Assert.AreEqual(100f, i.DisplayFeltTime);
            i.UpdateFeltTime(-20.5f);
            Assert.AreEqual(79.5f, i.DisplayFeltTime);
            i.UpdateFeltTime(-100000f);
            Assert.AreEqual(0f, i.DisplayFeltTime);
        }

        [Test]
        public void DisplayFeltTime()
        {
            // 初期値は開始時間であることを保証する
            var i = CreateInstance();
            i.SetFeltTerm(10f);
            Assert.AreEqual(i.FeltTerm.BeginningTime, i.DisplayFeltTime);
            i.SetFeltTerm(20f, 30f);
            Assert.AreEqual(i.FeltTerm.BeginningTime, i.DisplayFeltTime);

            // 浮動小数展の表示を保証する
            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(1.23f);
            Assert.AreEqual(21.2f, i.DisplayFeltTime);

            // 丸めを保証する
            // 偶数丸めであることに注意する
            // 丸め位置である小数第一位(X.YZ の Z)が 5 のとき
            // 小数第二位 (X.YZ の Y）が奇数の場合は切り下げ / 偶数の場合は切り下げ 
            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.54f);
            Assert.AreEqual(24.5f, i.DisplayFeltTime);

            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.55f);
            Assert.AreEqual(24.5f, i.DisplayFeltTime);

            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.56f);
            Assert.AreEqual(24.6f, i.DisplayFeltTime);

            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.44f);
            Assert.AreEqual(24.4f, i.DisplayFeltTime);

            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.45f);
            Assert.AreEqual(24.5f, i.DisplayFeltTime);

            i = CreateInstance();
            i.SetFeltTerm(20f, 30f);
            i.UpdateFeltTime(4.46f);
            Assert.AreEqual(24.5f, i.DisplayFeltTime);
        }

        [Test]
        public void Progress()
        {
            // 初期値は開始時間であることを保証する
            var i = CreateInstance();
            i.SetFeltTerm(10f);
            Assert.AreEqual(0f, i.Progress);
            i.SetFeltTerm(20f, 400f);
            Assert.AreEqual(5f, i.Progress);

            // 増減
            i = CreateInstance();
            i.SetFeltTerm(20f, 100f);
            Assert.AreEqual(20f, i.Progress);
            i.UpdateFeltTime(20f);
            Assert.AreEqual(40f, i.Progress);

            i = CreateInstance();
            i.SetFeltTerm(100f, 0f);
            i.UpdateFeltTime(-10f);
            Assert.AreEqual(10f, i.Progress);

            // 上限 / 下限
            i = CreateInstance();
            i.SetFeltTerm(20f, 100f);
            i.UpdateFeltTime(1000f);
            Assert.AreEqual(100f, i.Progress);
            i.UpdateFeltTime(-1000f);
            Assert.AreEqual(0f, i.Progress);

            // 浮動小数展の表示を保証する
            i = CreateInstance();
            i.SetFeltTerm(100f);
            i.UpdateFeltTime(1.23f);
            Assert.AreEqual(1.2f, i.Progress);

            // 丸めを保証する
            i = CreateInstance();
            i.SetFeltTerm(100f);
            i.UpdateFeltTime(4.54f);
            Assert.AreEqual(4.5f, i.Progress);

            i = CreateInstance();
            i.SetFeltTerm(100f);
            i.UpdateFeltTime(4.55f);
            Assert.AreEqual(4.6f, i.Progress);
        }
    }
}