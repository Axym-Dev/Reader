using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reader.Data.Product;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Reader.Data.Reading;
using Blazored.LocalStorage;

namespace Reader.Modules.Reading;

public class ReaderConfigManager
{
    public ReaderConfig Config;
    private SiteInteraction SiteInteraction;
    private Action SetupTextPieces;
    private ILocalStorageService localStorage;
    public ReaderConfigManager(ReaderConfig config, SiteInteraction siteInteraction, Action setupTextPieces, ILocalStorageService localStorage)
    {
        Config = config;
        SiteInteraction = siteInteraction;
        SetupTextPieces = setupTextPieces;
        this.localStorage = localStorage;
    }

    public int FrontReadingSpeed
    {
        get => Config.ReadingSpeed;
        set
        {
            Config.ReadingSpeed = value;
            SaveConfig();
        }
    }

    public int FrontPeripheralCharsCount
    {
        get => Config.PeripheralCharsCount;
        set
        {
            Config.PeripheralCharsCount = value;
            SaveConfig();
        }
    }

    public int FrontPeripheralWordsBrightness
    {
        get => Config.PeripheralWordsBrightness;
        set
        {
            Config.PeripheralWordsBrightness = value;
            SaveConfig();
        }
    }

    public int FrontWordNavCount
    {
        get => Config.WordNavCount;
        set
        {
            Config.WordNavCount = value;
            SaveConfig();
        }
    }

    public int FrontTextSize
    {
        get => Config.TextSize;
        set
        {
            Config.TextSize = value;
            SaveConfig();
        }
    }

    public string FrontFont
    {
        get => string.Join(" ", Regex.Split(Config.Font.ToString(), @"(?<!^)(?=[A-Z])"));
        set
        {
            Config.Font = (Font)Enum.Parse(typeof(Font), value.Replace(" ", ""));
            SaveConfig();
        }
    }

    public List<string> FontOptions = Enum
        .GetValues(typeof(Font))
        .Cast<Font>()
        .Select(x => string.Join(" ", Regex.Split(x.ToString(), @"(?<!^)(?=[A-Z])")))
        .ToList();

    public int FrontFontWeight
    {
        get => Config.FontWeight;
        set
        {
            Config.FontWeight = value;
            SaveConfig();
        }
    }

    public int FrontWordCharLimit
    {
        get => Config.WordCharLimit;
        set
        {
            Config.WordCharLimit = value;
            SaveConfig();
            SetupTextPieces();
        }
    }

    public string FrontMiddleCharHighlightingColor
    {
        get => Config.MiddleCharHighlightingColor;
        set
        {
            Config.MiddleCharHighlightingColor = value;
            SaveConfig();
        }
    }

    public bool FrontRightToLeft
    {
        get => Config.RightToLeft;
        set
        {
            Config.RightToLeft = value;
            SaveConfig();
        }
    }

    public void SaveConfig()
    {
        _ = Task.Run(() => localStorage.SetItemAsStringAsync("readerConfig", JsonConvert.SerializeObject(Config)));
    }

}
