using System;

namespace com.amabie.EngrossingTime {
    /// <summary>
	/// 没頭する時間クラス
	/// ゲーム上の時間と体感時間を指定することで
	/// </summary>
    public class EngrossingTime
    {
        /// <summary>
		/// 体感期間(秒)
        /// 開始時間と終了時間を保持する。
        /// 0 以上の数値が入る。
		/// </summary>
        public Term FeltTerm { get; private set; }

        /// <summary>
		/// 体感時間(秒)
        /// 0 以上の数値が入る。
		/// </summary>
        private float feltTime;

        /// <summary>
        /// 体感時間が増えるかどうか
        /// 減る場合は UpdateFeltTime の引数はマイナスであることを想定している
        /// </summary>
        private bool isIncrement { get { return FeltTerm.BeginningTime <= FeltTerm.EndingTime; } }

        /// <summary>
        /// 表示用の体感時間(秒)
        /// 偶数丸め処理で出力する。
        /// 0 以上の浮動小数。小数第一位まで。
        /// </summary>
        public float DisplayFeltTime {
            get
            {
                return (float)Math.Round(feltTime, 1, MidpointRounding.AwayFromZero);
            }
        }
        /// <summary>
		/// 進捗度(%)
        /// 0 以上の浮動小数。小数第一位まで。
		/// </summary>
        public float Progress
        {
            get {
                if (isIncrement)
                {
                    if (feltTime >= FeltTerm.EndingTime)
                    {
                        return 100f;
                    }
                    return (float)Math.Round(
                        feltTime / FeltTerm.EndingTime * 100f,
                        1,
                        MidpointRounding.AwayFromZero
                    );
                }
                else
                {
                    if (feltTime <= FeltTerm.EndingTime)
                    {
                        return 100f;
                    }
                    return (float)Math.Round(
                        (1f - (feltTime - FeltTerm.EndingTime) / (FeltTerm.BeginningTime - FeltTerm.EndingTime)) * 100f,
                        1,
                        MidpointRounding.AwayFromZero
                    );
                }
            }
        }

        public EngrossingTime()
        {
            feltTime = 0;
            FeltTerm = new Term(0, 0);
        }

        /// <summary>
        /// 体感期間を設定する
        /// 3分で1日を表現したい場合は(0, 180) を引数に与える
        /// </summary>
        /// <example>
        /// ゲームのシナリオ都合で
        /// 普通は180秒だが、特殊処理で一日
        /// </example>
        /// <param name="beginningTime">開始時間</param>
        /// <param name="endingTime">終了時間</param>
        public void SetFeltTime(float beginningTime, float endingTime)
        {
            FeltTerm = new Term(beginningTime, endingTime);
            feltTime = beginningTime;
        }

        /// <summary>
        /// 体感期間を設定する
        /// 0秒〜計測する場合は引数一つで設定できるようにした
        /// </summary>
        /// <param name="endingTime">終了時間</param>
        public void SetFeltTime(float endingTime)
        {
            SetFeltTime(0, endingTime);
        }

        /// <summary>
		/// 体感時間を更新する
		/// Lifecycle の Update メソッドで呼ばれることを想定している
		/// </summary>
		/// <param name="deltaTime">Time.deltaTime を想定している</param>
        public void UpdateFletTime(float deltaTime)
        {
            feltTime += deltaTime;
            if (isIncrement)
            {
                if (feltTime < 0)
                {
                    feltTime = 0;
                }
                if (feltTime > FeltTerm.EndingTime)
                {
                    feltTime = FeltTerm.EndingTime;
                }
            }
            else
            {
                if (feltTime < 0)
                {
                    feltTime = 0;
                }
                if (feltTime > FeltTerm.BeginningTime)
                {
                    feltTime = FeltTerm.BeginningTime;
                }
            }
        }

        /// <summary>
        /// 期間を表現するサブクラス
        /// </summary>
        public class Term
        {
            public float BeginningTime { get; private set; }
            public float EndingTime { get; private set; }

            public Term(float beginningTime, float endingTime)
            {
                BeginningTime = beginningTime;
                EndingTime = endingTime;
            }
        }
    }
}