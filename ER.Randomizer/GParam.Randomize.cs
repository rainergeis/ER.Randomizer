using SoulsFormats;
using SoulsFormats.KF4;
using System.IO;
using System.Numerics;
using WitchyFormats;
using Group = WitchyFormats.GPARAM.Group;
using Param = WitchyFormats.GPARAM.Param;

namespace ER.Randomizer;

public partial class GParam_Randomize : Form
{
    public GParam_Randomize()
    {
        // Prompts the user to select a GPARAM file(s) to randomize
        OpenFileDialog dialog = new OpenFileDialog
        {
            Title = @"Select the GPARAM files you want to randomize!",
            Multiselect = true
        };

        // Proceed onwards if the user selects the OK button
        if (dialog.ShowDialog() != DialogResult.OK) return;

        foreach (string filePath in dialog.FileNames)
        {
            try
            {
                /*if (!File.Exists(filePath))
                    continue;
                File.WriteAllBytes(filePath, Resource1.gparam_template);*/

                WitchyFormats.GPARAM gparam = WitchyFormats.GPARAM.Read(filePath);
                RandomizeParams(gparam);

                if (!File.Exists($"{filePath}.bak"))
                {
                    File.Move(filePath, $"{filePath}.bak");
                }
                gparam.Write(filePath, DCX.Type.DCX_KRAK);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
        Console.WriteLine(@"Done!");
    }  

    void RandomizeParams(WitchyFormats.GPARAM file)
    {        
        foreach (Group group in file.Groups)     
        {
            string groupName = group.Name2;
            if (groupName == "Tone Map")
            {
                RandomizeToneMap(group);
            }
            else if (groupName == "Bloom")
            {
                RandomizeBloom(group);   
            }
        }
    }

    Single randomFloatInRange(Single min, Single max)
    {
        return new Random().NextSingle() * (max - min) + min;
    }

    void RandomizeToneMap(Group group)
    {
        Param? exposure = group.Params.Find(param => param.Name2 == "Exposure");
        if (exposure != null)
        {
            for (int i = 0; i < exposure.Values.Count; i++) 
            { 
                exposure.Values[i] = Random.Shared.NextSingle() * 10f; 
            }
        }

        Param? aberrationEnable = group.Params.Find(param => param.Name2 == "Enable Chromatic Aberration");
        if (aberrationEnable != null)
        {
            aberrationEnable.Values[0] = 1;
        }

        Param? ToneLateralDispersion = group.Params.Find(param => param.Name2 == "ToneLateralDispersion[0]");
        if (ToneLateralDispersion != null)
        {
            ToneLateralDispersion.Values[0] = Random.Shared.NextSingle() * 0.1f;
        }

        Param? ToneVerticalDispersion = group.Params.Find(param => param.Name2 == "ToneLateralDispersion[1]");
        if (ToneVerticalDispersion != null)
        {
            ToneVerticalDispersion.Values[0] = Random.Shared.NextSingle() * 0.1f;
        }

        Param? ColorShift = group.Params.Find(param => param.Name2 == "ColorShift");
        if (ColorShift != null)
        {
            var vector = (Vector4)ColorShift.Values[0];
            vector.X = randomFloatInRange(0.2f, 0.8f);
            vector.Y = randomFloatInRange(0.2f, 0.8f);
            vector.Z = randomFloatInRange(0.2f, 0.8f);
            vector.W = 1f;
            ColorShift.Values[0] = vector;
        }
    }

    void RandomizeBloom(Group group)
    {
        Param? bloomEnable = group.Params.Find(param => param.Name2 == "Enable");
        if (bloomEnable != null)
        {
            for (int i = 0; i < bloomEnable.Values.Count; i++)
            {
                bloomEnable.Values[i] = 1;
            }
        }

        Param? glareNumLevels = group.Params.Find(param => param.Name2 == "GlareNumLevels");
        if (glareNumLevels != null)
        {
            for (int i = 0; i < glareNumLevels.Values.Count; i++)
            {
                glareNumLevels.Values[i] = new Random().Next(0, 25);
            }
                
        }

        Param? glareLuminance = group.Params.Find(param => param.Name2 == "Glare Luminance");
        if (glareLuminance != null)
        {
            for (int i = 0; i < glareLuminance.Values.Count; i++)
            {
                glareLuminance.Values[i] = randomFloatInRange(0.2f, 0.99f) * 20f;
            }

        }

        Param? glareBloomAlpha = group.Params.Find(param => param.Name2 == "GlareBloom Alpha");
        if (glareBloomAlpha != null)
        {
            for (int i = 0; i < glareBloomAlpha.Values.Count; i++)
            {
                glareBloomAlpha.Values[i] = 1f;
            }

        }

        Param? glareBloomGaussian = group.Params.Find(param => param.Name2 == "GlareBloom GaussianRadiusScale");
        if (glareBloomGaussian != null)
        {
            for (int i = 0; i < glareBloomGaussian.Values.Count; i++)
            {
                glareBloomGaussian.Values[i] = new Random().NextSingle() * 20f;
            }

        }

        Param? glareBloomSaturate = group.Params.Find(param => param.Name2 == "GlareBloom Saturate Ctrl");
        if (glareBloomSaturate != null)
        {
            for (int i = 0; i < glareBloomSaturate.Values.Count; i++)
            {
                glareBloomSaturate.Values[i] = new Random().NextSingle() * 20f;
            }

        }

        Param? glareBloomSmooth = group.Params.Find(param => param.Name2 == "GlareBloom Smooth");
        if (glareBloomSmooth != null)
        {
            for (int i = 0; i < glareBloomSmooth.Values.Count; i++)
            {
                glareBloomSmooth.Values[i] = new Random().NextSingle() * 20f;
            }

        }

        Param? glareBloomFeedback = group.Params.Find(param => param.Name2 == "GlareBloom FeedbackWeight");
        if (glareBloomFeedback != null)
        {
            for (int i = 0; i < glareBloomFeedback.Values.Count; i++)
            {
                glareBloomFeedback.Values[i] = new Random().NextSingle() * 20f;
            }

        }
    }
}
