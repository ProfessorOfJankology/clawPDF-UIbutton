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
            ProfileGuid = "DefaultGuid";
        }

        public void ReadValues(Data data, string path)
        {
            try
            {
                Mode = (UIActionButtonMode)int.Parse(data.GetValue(@"" + path + @"Mode"));
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
                ProfileGuid = Data.UnescapeString(data.GetValue(@"" + path + @"ProfileGuid"));
            }
            catch
            {
                ProfileGuid = "DefaultGuid";
            }
        }

        public void StoreValues(Data data, string path)
        {
            data.SetValue(@"" + path + @"Mode", ((int)Mode).ToString());
            data.SetValue(@"" + path + @"Label", Data.EscapeString(Label));
            data.SetValue(@"" + path + @"ProfileGuid", Data.EscapeString(ProfileGuid));
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
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
    }
}
