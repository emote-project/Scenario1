using EmoteCommonMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IA
{
    public class UserState
    {
        public Charge mValence { get; set; }
        public Charge mArousal { get; set; }
        public Charge mTaskEngagement { get; set; }
        public Charge mSocialEngagement { get; set; }

        public double mValenceCI { get; set; }
        public double mArousalCI { get; set; }
        public double mTaskEngagementCI { get; set; }
        public double mSocialEngagementCI { get; set; }

        public PointofFocus mAttentionFocus { get; set; }
        public Charge mAttention { get; set; }

        public UserState()
        {
            mValence = Charge.Neutral;
            mArousal = Charge.Neutral;
            mTaskEngagement = Charge.Neutral;
            mSocialEngagement = Charge.Neutral;

            mAttentionFocus = PointofFocus.NonApplicable;
            mAttention = Charge.Neutral;
        }

        public UserState(UserState other)
        {
            mValence = other.mValence;
            mArousal = other.mArousal;
            mTaskEngagement = other.mTaskEngagement;
            mSocialEngagement = other.mSocialEngagement;

            mAttentionFocus = other.mAttentionFocus;
            mAttention = other.mAttention;
        }

        public bool Compare(UserState other)
        {
            return ((mValence != other.mValence) || (mArousal != other.mArousal) || (mTaskEngagement != other.mTaskEngagement) ||
                 (mSocialEngagement != other.mSocialEngagement) || (mAttentionFocus != other.mAttentionFocus) || (mAttention != other.mAttention) 
                 //|| (mValenceCI > other.mValenceCI) || (mArousalCI != other.mArousalCI) || (mSocialEngagementCI != other.mSocialEngagementCI) || (mTaskEngagementCI != other.mTaskEngagementCI)
                 );
        }
    }
}
