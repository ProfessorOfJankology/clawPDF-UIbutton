using System;
using System.Text;
using clawSoft.clawPDF.Core.Settings.Enums;
using pdfforge.DataStorage;

namespace clawSoft.clawPDF.Core.Settings
{
    public class UIActionButtonSettings
    {
        public UIActionButtonSettings()
        {
            Init();
        }

        public UIActionButtonMode Mode { get; set; }

        // Only used when Mode == Action
        public string Label { get; set; }

        // Profile GUID to auto-run
        public string ProfileGuid { get; set; }

        private void Init()
        {
            Mode = UIActionButtonMode.Email;
            Label = "";
            ProfileGuid = ProfileGuids.DEFAULT_PROFILE_GUID;
        }

        public void ReadValues(Data data, string path)
        {
            try
            {
                var modeValue = data.GetValue(@"" + path + @"Mode");
                if (!string.IsNullOrWhiteSpace(modeValue) && modeValue.Contains("."))
                    modeValue = modeValue.Substring(modeValue.LastIndexOf(".", StringComparison.Ordinal) + 1);
                if (!Enum.TryParse(modeValue, out UIActionButtonMode parsedMode))
                {
                    if (int.TryParse(modeValue, out var modeInt))
                        parsedMode = (UIActionButtonMode)modeInt;
                    else
                        parsedMode = UIActionButtonMode.Email;
                }

                Mode = parsedMode;
            }
            catch
            {
                Mode = UIActionButtonMode.Email;
            }

            try
            {
                Label = Data.UnescapeString(data.GetValue(@"" + path + @"Label"));
            }
            catch
            {
                Label = "";
            }

            try
            {
                var pg = Data.UnescapeString(data.GetValue(@"" + path + @"ProfileGuid"));
                ProfileGuid = string.IsNullOrWhiteSpace(pg) ? ProfileGuids.DEFAULT_PROFILE_GUID : pg;
            }
            catch
            {
                ProfileGuid = ProfileGuids.DEFAULT_PROFILE_GUID;
            }

        }

        public void StoreValues(Data data, string path)
        {
            data.SetValue(@"" + path + @"Mode", Mode.ToString());
            data.SetValue(@"" + path + @"Label", Data.EscapeString(Label));
var pg = string.IsNullOrWhiteSpace(ProfileGuid) ? ProfileGuids.DEFAULT_PROFILE_GUID : ProfileGuid;
data.SetValue(@"" + path + @"ProfileGuid", Data.EscapeString(pg));

        }

        public UIActionButtonSettings Copy()
        {
            return new UIActionButtonSettings
            {
                Mode = Mode,
                Label = Label,
                ProfileGuid = ProfileGuid
            };
        }

        public override bool Equals(object o)
        {
            if (!(o is UIActionButtonSettings)) return false;
            var v = (UIActionButtonSettings)o;

            if (!Mode.Equals(v.Mode)) return false;
            if (!string.Equals(Label, v.Label)) return false;
            if (!string.Equals(ProfileGuid, v.ProfileGuid)) return false;

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Mode=" + Mode);
            sb.AppendLine("Label=" + Label);
            sb.AppendLine("ProfileGuid=" + ProfileGuid);
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + Mode.GetHashCode();
                hash = hash * 23 + (Label?.GetHashCode() ?? 0);
                hash = hash * 23 + (ProfileGuid?.GetHashCode() ?? 0);
                return hash;
            }
        }

    }
}
