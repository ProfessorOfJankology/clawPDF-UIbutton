using System;
using System.Collections.Generic;
using System.Linq;
using clawSoft.clawPDF.Core.Settings;
using clawSoft.clawPDF.Core.Settings.Enums;
using clawSoft.clawPDF.Shared.Helper;
using pdfforge.DynamicTranslator;

namespace clawSoft.clawPDF.ViewModels.UserControls
{
    internal class GeneralTabViewModel : ApplicationSettingsViewModel
    {
        private ApplicationProperties _applicationProperties;
        private IEnumerable<ConversionProfile> _conversionProfiles;
        private IList<Language> _languages;

        public GeneralTabViewModel()
        {
            Languages = Translator.FindTranslations(TranslationHelper.Instance.TranslationPath);
        }

        public ApplicationProperties ApplicationProperties
        {
            get => _applicationProperties;

            set
            {
                _applicationProperties = value;
                RaisePropertyChanged("ApplicationProperties");
            }
        }

        public IList<Language> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                RaisePropertyChanged("Languages");
            }
        }

        public IEnumerable<AskSwitchPrinter> AskSwitchPrinterValues =>
            new List<AskSwitchPrinter>
            {
                new AskSwitchPrinter(
                    TranslationHelper.Instance.TranslatorInstance.GetTranslation("ApplicationSettingsWindow", "Ask",
                        "Ask"), true),
                new AskSwitchPrinter(
                    TranslationHelper.Instance.TranslatorInstance.GetTranslation("ApplicationSettingsWindow", "Yes",
                        "Yes"), false)
            };

        public IEnumerable<AskPrinterDialogTopMost> AskPrinterDialogTopMostValues =>
            new List<AskPrinterDialogTopMost>
            {
                new AskPrinterDialogTopMost(
                    TranslationHelper.Instance.TranslatorInstance.GetTranslation("ApplicationSettingsWindow", "Yes",
                        "Yes"), true),
                new AskPrinterDialogTopMost(
                    TranslationHelper.Instance.TranslatorInstance.GetTranslation("ApplicationSettingsWindow", "No",
                         "No"), false)
            };

        public IEnumerable<EnumValue<UpdateInterval>> UpdateIntervals =>
            TranslationHelper.Instance.TranslatorInstance.GetEnumTranslation<UpdateInterval>();

        public IEnumerable<EnumValue<Theme>> ThemeValues =>
            TranslationHelper.Instance.TranslatorInstance.GetEnumTranslation<Theme>();

        public IEnumerable<ConversionProfile> ConversionProfiles
        {
            get => _conversionProfiles ?? Enumerable.Empty<ConversionProfile>();
            set
            {
                _conversionProfiles = value ?? Enumerable.Empty<ConversionProfile>();
                RaisePropertyChanged(nameof(ActionButtonProfiles));
            }
        }

        public IEnumerable<ConversionProfile> ActionButtonProfiles =>
            ConversionProfiles.OrderBy(profile => profile.Name).ToList();

        public bool LanguageIsEnabled => true;

        public string CurrentLanguage
        {
            get => ApplicationSettings.Language;
            set => ApplicationSettings.Language = value;
        }

        public bool ActionButtonUsesDefaultEmail
        {
            get => ApplicationSettings?.UIActionButton?.Mode == UIActionButtonMode.Email;
            set
            {
                if (value)
                    SetActionButtonMode(UIActionButtonMode.Email);
            }
        }

        public bool ActionButtonUsesCustom
        {
            get => ApplicationSettings?.UIActionButton?.Mode == UIActionButtonMode.Action;
            set
            {
                if (value)
                    SetActionButtonMode(UIActionButtonMode.Action);
            }
        }

        public bool ActionButtonCustomOptionsEnabled => ActionButtonUsesCustom;

        public string ActionButtonLabel
        {
            get => ApplicationSettings?.UIActionButton?.Label ?? string.Empty;
            set
            {
                EnsureActionButtonSettings();
                ApplicationSettings.UIActionButton.Label = value ?? string.Empty;
                RaisePropertyChanged(nameof(ActionButtonLabel));
            }
        }

        public string ActionButtonProfileGuid
        {
            get => ApplicationSettings?.UIActionButton?.ProfileGuid ?? ProfileGuids.DEFAULT_PROFILE_GUID;
            set
            {
                EnsureActionButtonSettings();
                ApplicationSettings.UIActionButton.ProfileGuid = value ?? string.Empty;
                RaisePropertyChanged(nameof(ActionButtonProfileGuid));
            }
        }

        protected override void OnSettingsChanged(EventArgs e)
        {
            base.OnSettingsChanged(e);

            RaisePropertyChanged("Languages");
            RaisePropertyChanged("CurrentLanguage");
            RaisePropertyChanged("LanguageIsEnabled");
            RaisePropertyChanged("CurrentUpdateInterval");
            RaisePropertyChanged("UpdateIsEnabled");
            RaisePropertyChanged(nameof(ActionButtonUsesDefaultEmail));
            RaisePropertyChanged(nameof(ActionButtonUsesCustom));
            RaisePropertyChanged(nameof(ActionButtonCustomOptionsEnabled));
            RaisePropertyChanged(nameof(ActionButtonLabel));
            RaisePropertyChanged(nameof(ActionButtonProfileGuid));
        }

        public void UpdateIntervalChanged()
        {
            RaisePropertyChanged("DisplayUpdateWarning");
        }

        private void SetActionButtonMode(UIActionButtonMode mode)
        {
            if (ApplicationSettings == null)
                return;

            EnsureActionButtonSettings();
            if (ApplicationSettings.UIActionButton.Mode == mode)
                return;

            ApplicationSettings.UIActionButton.Mode = mode;
            RaisePropertyChanged(nameof(ActionButtonUsesDefaultEmail));
            RaisePropertyChanged(nameof(ActionButtonUsesCustom));
            RaisePropertyChanged(nameof(ActionButtonCustomOptionsEnabled));
        }

        private void EnsureActionButtonSettings()
        {
            if (ApplicationSettings == null)
                return;

            if (ApplicationSettings.UIActionButton == null)
                ApplicationSettings.UIActionButton = new UIActionButtonSettings();
        }
    }
}
