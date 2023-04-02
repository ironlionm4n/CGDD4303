/*
GRADE MANAGER
Calculates the grade for each phase, shows them on the screen, then saves to a file
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;
using TMPro;

public class GradeManager : MonoBehaviour
{
    [Header("Points Per Section")] public int shopWeight = 400;
    public int cutWeight = 400;
    public int assembleWeight = 400;

    [Header("Visit Reductions")] public float extraShopReduction = .25f;

    [Header("Efficient Purchase Quantities")]
    // public int efficientPlywood = 14;
    public int efficientPlywood = 12;

    public int efficient2x4 = 22;
    public int efficient2x6 = 12;
    public int efficient4x4 = 36;
    public int efficientStrut = 3;
    public int efficientTie = 42;
    public int efficientClamp = 8;
    public float qtyDifferenceReduction = .1f;

    [Header("Minimum Waste Amounts")]
    //public float minimumWastePlywood = 84;
    public float minimumWaste2x4 = 55;

    public float minimumWaste2x6 = 0;
    public float minimumWaste4x4 = 56.30f;
    public float minimumWastePlywood = 84;
    public float minimumWasteStrut = 4.5f;

    [Header("Correct Layer Order")] public LayerSelection.Layer[] correctLayers;

    [Header("Time Requirements")]
    public float[] secondsForGrading = new float[5] {600f, 1200f, 1800f, 2400f, float.PositiveInfinity};

    public float[] timeScorePercents = new float[5] {1, .9f, .8f, .7f, .5f};

    [Header("UI Display")] public GameObject scoreScreen;
    public Text shopScoreText;
    public TMP_Text shopScoreTMP;
    public Text shopGradeText;
    public TMP_Text shopGradeTMP;
    public Text cutScoreText;
    public TMP_Text cutScoreTMP;
    public Text cutGradeText;
    public TMP_Text cutGradeTMP;
    public Text assembleScoreText;
    public TMP_Text assembleScoreTMP;
    public Text assembleGradeText;
    public TMP_Text assembleGradeTMP;
    public Text totalScoreText;
    public TMP_Text totalScoreTMP;
    public Text totalGradeText;
    public TMP_Text totalGradeTMP;

    [Header("Grade Level Points")] public float[] gradeBoundaries = new float[5] {1, .9f, .8f, .7f, .5f};
    public char[] grades = new char[5] {'A', 'B', 'C', 'D', 'F'};

    [Header("Saving")] public string saveFile = "saveData.csv";
    public string[] fileHeaders = new string[5] {"SHOP", "CUT", "ASSEMBLE", "TOTAL", "TIME"};

    private int extraShopVisits = 0;

    private int totalPlywood = 0;
    private int total2x4 = 0;
    private int total2x6 = 0;
    private int total4x4 = 0;
    private int totalTie = 0;
    private int totalStrut = 0;
    private int totalClamp = 0;

    private float wastePlywood = 0;
    private float waste2x4 = 0;
    private float waste2x6 = 0;
    private float waste4x4 = 0;
    private float wasteStrut = 0;

    private float finalTime = 0;
    private bool layerFail = false;

    private float shopPoints = 0;
    private float cutPoints = 0;
    private float assemblePoints = 0;

    private float totalPoints = 0;

    private List<LayerSelection.Layer> layerOrder = new List<LayerSelection.Layer>();

    [Header("Formwork")] public Formwork type;

    [Header("Confetti")] [SerializeField] private ParticleSystem confetti;

    [Header("Explosion")] [SerializeField] private Explode explode;
    [SerializeField] private float suspenseTime = 3f;
    [SerializeField] private float timeTillResults = 3f;
    [SerializeField] private AudioSource suspenseMusic;
    [SerializeField] private BackgroundMusic backgroundMusic;
    [SerializeField] private AudioSource congrats;
    [SerializeField] private AudioSource congrats2;

    [Header("Events")] [SerializeField] private GameEvent hideLayers;

    public enum Formwork
    {
        Slab,
        Wall,
        Column
    }

    private void Start()
    {
        scoreScreen.SetActive(false);
    }

    #region Set Grade While Playing

    /// <summary>
    /// Adds a visited layer to the list
    /// </summary>
    /// <param name="l">Layer to add</param>
    public void AddLayer(LayerSelection.Layer l)
    {
        layerOrder.Add(l);
    }

    /// <summary>
    /// Stores how many total of each material has been purchased
    /// </summary>
    /// <param name="ply">Amount of plywood</param>
    /// <param name="twoByFour">Amount of 2x4</param>
    /// <param name="twoBySix">Amount of 2x6</param>
    /// <param name="fourByFour">Amount of 4x4</param>
    public void StoreCheckout(int ply, int twoByFour, int twoBySix, int fourByFour, int tie, int strut, int clamp)
    {
        totalPlywood += ply;
        total2x4 += twoByFour;
        total2x6 += twoBySix;
        total4x4 += fourByFour;
        totalTie += tie;
        totalStrut += strut;
        totalClamp += clamp;
    }

    /// <summary>
    /// Stores how much waste has been created by cutting
    /// </summary>
    /// <param name="plyWaste">Waste plywood</param>
    /// <param name="twoByFourWaste">Waste 2x4</param>
    /// <param name="twoBySixWaste">Waste 2x6</param>
    /// <param name="fourByFourWaste">Waste 4x4</param>
    public void CutCheckout(float plyWaste, float twoByFourWaste, float twoBySixWaste, float fourByFourWaste,
        float strutWaste)
    {
        wastePlywood += plyWaste;
        waste2x4 += twoByFourWaste;
        waste2x6 += twoBySixWaste;
        waste4x4 += fourByFourWaste;
        wasteStrut += strutWaste;
    }

    /// <summary>
    /// Stores how many times the shop has been visited
    /// </summary>
    public void VisitShop()
    {
        extraShopVisits++;
    }

    #endregion

    #region Calculate Point Totals

    /// <summary>
    /// Calculates how many points earned in the shop phase
    /// </summary>
    private void ShopPointsCalculation()
    {
        //Difference between efficient and actual
        int differencePly = Mathf.Abs(efficientPlywood - totalPlywood);
        // int differencePly = Mathf.Abs(12 - totalPlywood);
        int difference2x4 = Mathf.Abs(efficient2x4 - total2x4);
        int difference2x6 = Mathf.Abs(efficient2x6 - total2x6);
        int difference4x4 = Mathf.Abs(efficient4x4 - total4x4);

        Debug.Log(differencePly);

        //Percentage score for each type
        //If they get it really wrong, make sure it doesn't go below 0
        float scorePly = 1 - (differencePly * qtyDifferenceReduction);
        if (scorePly < 0)
        {
            scorePly = 0;
        }

        float score2x4 = 1 - (difference2x4 * qtyDifferenceReduction);
        if (score2x4 < 0)
        {
            score2x4 = 0;
        }

        float score2x6 = 1 - (difference2x6 * qtyDifferenceReduction);
        if (score2x6 < 0)
        {
            score2x6 = 0;
        }

        float score4x4 = 1 - (difference4x4 * qtyDifferenceReduction);
        if (score4x4 < 0)
        {
            score4x4 = 0;
        }

        //Total score
        float avgScore = (scorePly + score2x4 + score2x6 + score4x4) / 4;
        shopPoints = (avgScore * shopWeight);

        //4 or more extra shop visits should result in a 100% point reduction
        if (extraShopVisits > 0 && extraShopVisits < 4)
        {
            shopPoints -= shopPoints * (extraShopVisits * extraShopReduction);
        }
        else if (extraShopVisits > 3)
        {
            shopPoints = 0;
        }
    }

    private void WallShopPointsCalculation()
    {
        //Difference between efficient and actual
        int differencePly = Mathf.Abs(efficientPlywood - totalPlywood);
        // int differencePly = Mathf.Abs(12 - totalPlywood);
        int difference2x4 = Mathf.Abs(efficient2x4 - total2x4);
        int difference2x6 = Mathf.Abs(efficient2x6 - total2x6);
        int differenceStrut = Mathf.Abs((efficientStrut * 10) - (totalStrut * 10));
        int differenceTie = Mathf.Abs(efficientTie - totalTie);

        //Percentage score for each type
        //If they get it really wrong, make sure it doesn't go below 0
        float scorePly = 1 - (differencePly * qtyDifferenceReduction);
        if (scorePly < 0)
        {
            scorePly = 0;
        }

        float score2x4 = 1 - (difference2x4 * qtyDifferenceReduction);
        if (score2x4 < 0)
        {
            score2x4 = 0;
        }

        float score2x6 = 1 - (difference2x6 * qtyDifferenceReduction);
        if (score2x6 < 0)
        {
            score2x6 = 0;
        }

        float scoreStrut = 1 - (differenceStrut * qtyDifferenceReduction);
        if (scoreStrut < 0)
        {
            scoreStrut = 0;
        }

        float scoreTie = 1 - (differenceTie * qtyDifferenceReduction);
        if (scoreTie < 0)
        {
            scoreTie = 0;
        }

        //Total score
        float avgScore = (scorePly + score2x4 + score2x6 + scoreStrut + scoreTie) / 5;
        shopPoints = (avgScore * shopWeight);

        //4 or more extra shop visits should result in a 100% point reduction
        if (extraShopVisits > 0 && extraShopVisits < 4)
        {
            shopPoints -= shopPoints * (extraShopVisits * extraShopReduction);
        }
        else if (extraShopVisits > 3)
        {
            shopPoints = 0;
        }
    }

    private void ColumnShopPointsCalculation()
    {
        //Difference between efficient and actual
        int differencePly = Mathf.Abs(efficientPlywood - totalPlywood);
        // int differencePly = Mathf.Abs(12 - totalPlywood);
        int difference2x4 = Mathf.Abs(efficient2x4 - total2x4);
        int differenceStrut = Mathf.Abs((efficientStrut * 10) - (totalStrut * 10));
        int differenceClamp = Mathf.Abs(efficientClamp - totalClamp);

        //Percentage score for each type
        //If they get it really wrong, make sure it doesn't go below 0
        float scorePly = 1 - (differencePly * qtyDifferenceReduction);
        if (scorePly < 0)
        {
            scorePly = 0;
        }

        float score2x4 = 1 - (difference2x4 * qtyDifferenceReduction);
        if (score2x4 < 0)
        {
            score2x4 = 0;
        }

        float scoreStrut = 1 - (differenceStrut * qtyDifferenceReduction);
        if (scoreStrut < 0)
        {
            scoreStrut = 0;
        }

        float scoreClamp = 1 - (differenceClamp * qtyDifferenceReduction);
        if (scoreClamp < 0)
        {
            scoreClamp = 0;
        }

        //Total score
        float avgScore = (scorePly + score2x4 + scoreStrut + scoreClamp) / 4;
        shopPoints = (avgScore * shopWeight);

        //4 or more extra shop visits should result in a 100% point reduction
        if (extraShopVisits > 0 && extraShopVisits < 4)
        {
            shopPoints -= shopPoints * (extraShopVisits * extraShopReduction);
        }
        else if (extraShopVisits > 3)
        {
            shopPoints = 0;
        }
    }

    /// <summary>
    /// Calculates how many points earned in the cut phase
    /// </summary>
    private void CutPointsCalculation()
    {
        //Percent of waste related to the minimum waste
        float percentWastePly = Percent(minimumWastePlywood, wastePlywood);
        float percentWaste2x4 = Percent(minimumWaste2x4, waste2x4);
        float percentWaste2x6 = Percent(minimumWaste2x6, waste2x6);
        float percentWaste4x4 = Percent(minimumWaste4x4, waste4x4);

        float tripReduction = extraShopVisits * extraShopReduction;

        float avgWaste = (percentWastePly + percentWaste2x4 + percentWaste2x6 + percentWaste4x4) / 4;
        Debug.Log(avgWaste);
        // cutPoints = (avgWaste - tripReduction) * cutWeight;
        // cutPoints = (avgWaste) * cutWeight * (1- tripReduction);
        //  cutPoints = (avgWaste) * cutWeight;
        // cutPoints = cutWeight;
        //cutPoints = minimumWastePlywood;

        if (avgWaste == 1)
        {
            cutPoints = 400;
            return;
        }

        cutPoints = cutWeight - avgWaste * 10;
    }

    private void WallCutPointsCalculation()
    {
        //Percent of waste related to the minimum waste
        float percentWastePly = Percent(minimumWastePlywood, wastePlywood);
        float percentWaste2x4 = Percent(minimumWaste2x4, waste2x4);
        float percentWaste2x6 = Percent(minimumWaste2x6, waste2x6);
        float percentWasteStrut = Percent(minimumWasteStrut, wasteStrut);

        float tripReduction = extraShopVisits * extraShopReduction;

        float avgWaste = (percentWastePly + percentWaste2x4 + percentWaste2x6 + percentWasteStrut) / 4;
        
        cutPoints = cutWeight * avgWaste;
    }

    private void ColumnCutPointsCalculation()
    {
        // Percent of waste related to the minimum waste
        float percentWastePly = Percent(minimumWastePlywood, wastePlywood);
        float percentWaste2x4 = Percent(minimumWaste2x4, waste2x4);
        float percentWasteStrut = Percent(minimumWasteStrut, wasteStrut);

        float tripReduction = extraShopVisits * extraShopReduction;

        float avgWaste = (percentWastePly + percentWaste2x4 + percentWasteStrut) / 3;
        cutPoints = cutWeight * avgWaste;
    }

    /// <summary>
    /// Calculates the percentage of a value
    /// </summary>
    /// <param name="efficient">Maximum value, ie 100%</param>
    /// <param name="actual">Actual value</param>
    /// <returns>Percentage</returns>
    private float Percent(float efficient, float actual)
    {
        if (efficient == actual)
        {
            return 1;
        }

        if (efficient != 0)
        {
            return (Mathf.Abs(actual - efficient)) / efficient;
            //return (efficient - Mathf.Abs(actual - efficient))/efficient;
        }
        else
        {
            return Mathf.Max(0, 1 - actual * qtyDifferenceReduction);
        }
    }

    /// <summary>
    /// Calculates how many points earned in the assembly phase
    /// </summary>
    private void AssemblePointsCalculation()
    {
        //Check if they have the right number of layers - if they don't, it's a fail
        if (layerOrder.Count != correctLayers.Length)
        {
            layerFail = true;
        }
        //Check for order of layers
        else
        {
            for (int i = 0; i < layerOrder.Count; i++)
            {
                if (layerOrder[i] != correctLayers[i])
                {
                    layerFail = true;
                    break;
                }
            }
        }

        if (!layerFail)
        {
            //Add in accuracy calculation here eventually

            float timePercent = 0;

            //Checks which time range the player was in, from shortest to longest
            //This is why the last value in timeScorePercents needs to be Float.Infinity
            for (int i = 0; i < timeScorePercents.Length; i++)
            {
                if (finalTime <= secondsForGrading[i])
                {
                    timePercent = timeScorePercents[i];
                    break;
                }
            }

            assemblePoints = timePercent * assembleWeight;
        }
        else
        {
            assemblePoints = 0;
        }
    }

    /// <summary>
    /// Calculates the total points earned by the player
    /// </summary>
    public void TotalPointsCalculation()
    {
        if (type == Formwork.Wall)
        {
            WallCutPointsCalculation();
            WallShopPointsCalculation();
        }
        else if (type == Formwork.Column)
        {
            ColumnShopPointsCalculation();
            ColumnCutPointsCalculation();
        }
        else
        {
            CutPointsCalculation();
            ShopPointsCalculation();
        }

        AssemblePointsCalculation();
        totalPoints = shopPoints + cutPoints + assemblePoints;
    }

    #endregion

    #region End Game

    /// <summary>
    /// Finalizes the grades and opens the grade screen
    /// </summary>
    public void EndGame()
    {
        //Disables all layers
        hideLayers.TriggerEvent();

        TotalPointsCalculation();

        StartCoroutine(Suspense());
    }

    public IEnumerator Suspense()
    {
        suspenseMusic.Play();
        backgroundMusic.StopMusic();

        //Debug.Log(totalPoints);
        yield return new WaitUntil(()=> !suspenseMusic.isPlaying);

        if (totalPoints == 1200)
        {
            if (confetti != null)
                confetti.Play();
    
            congrats.Play();
            StartCoroutine(ShowResults());
        }
        else if (assemblePoints == 400)
        {
            congrats2.Play();
            StartCoroutine(ShowResults());
        }
        else
        {
            if(explode != null)
                explode.ExplodeBuild();
            StartCoroutine(ShowResults());
        }
    }

    public IEnumerator ShowResults()
    {
        yield return new WaitForSeconds(timeTillResults);
        SetScoreUI();
        SaveToFile();
    }

    /// <summary>
    /// Shows all the scores in UI
    /// </summary>
    public void SetScoreUI()
    {
        scoreScreen.SetActive(true);
        shopScoreText.text = PointsText(shopPoints, shopWeight);
        shopScoreTMP.text = PointsText(shopPoints, shopWeight);
        shopGradeText.text = GradeText(shopPoints, shopWeight);
        shopGradeTMP.text = GradeText(shopPoints, shopWeight);
        cutScoreText.text = PointsText(cutPoints, cutWeight);
        cutScoreTMP.text = PointsText(cutPoints, cutWeight);
        cutGradeText.text = GradeText(cutPoints, cutWeight);
        cutGradeTMP.text = GradeText(cutPoints, cutWeight);

        if (!layerFail)
        {
            assembleScoreText.text = PointsText(assemblePoints, assembleWeight);
            assembleScoreTMP.text = PointsText(assemblePoints, assembleWeight);
            assembleGradeText.text = GradeText(assemblePoints, assembleWeight);
            assembleGradeTMP.text = GradeText(assemblePoints, assembleWeight);
            totalScoreText.text = PointsText(totalPoints, shopWeight + cutWeight + assembleWeight);
            totalScoreTMP.text = PointsText(totalPoints, shopWeight + cutWeight + assembleWeight);
            totalGradeText.text = GradeText(totalPoints, shopWeight + cutWeight + assembleWeight);
            totalGradeTMP.text = GradeText(totalPoints, shopWeight + cutWeight + assembleWeight);
        }
        else
        {
            assembleScoreText.text = "Points: FAIL / " + assembleWeight;
            assembleScoreTMP.text = $"Points: FAIL / {assembleWeight}";
            assembleGradeText.text = "Grade: F";
            assembleGradeTMP.text = "Grade: F";
            totalScoreText.text = "Points: FAIL / " + (shopWeight + cutWeight + assembleWeight);
            totalScoreTMP.text = "Points: FAIL / " + (shopWeight + cutWeight + assembleWeight);
            totalGradeText.text = "Grade: F";
            totalGradeTMP.text = "Grade: F";
        }
    }

    /// <summary>
    /// Creates a string formatted to show the points
    /// </summary>
    /// <param name="points">Points earned</param>
    /// <param name="outOf">Total possible points</param>
    /// <returns>Formatted string</returns>
    private string PointsText(float points, int outOf)
    {
        return $"Points: {points} / {outOf}";
    }

    /// <summary>
    /// Calculates the grade rank for the points earned
    /// </summary>
    /// <param name="points">Points earned</param>
    /// <param name="outOf">Total possible points</param>
    /// <returns>Formatted string</returns>
    private string GradeText(float points, int outOf)
    {
        float percent = points / outOf;
        percent = (float) Math.Round(percent, 3);

        for (int i = gradeBoundaries.Length - 1; i >= 0; i--)
        {
            if (percent <= gradeBoundaries[i])
            {
                return $"Grade: {grades[i]}";
            }
        }

        return "grade error";
    }

    /// <summary>
    /// Saves scores and final time to a file
    /// </summary>
    private void SaveToFile()
    {
        //If you had to make a new file, add the header to it
        if (!File.Exists(saveFile))
        {
            StreamWriter sw = new StreamWriter(saveFile);
            string header = "";
            for (int i = 0; i < fileHeaders.Length - 1; i++)
            {
                header += fileHeaders[i] + ",";
            }

            header += fileHeaders[fileHeaders.Length - 1];
            sw.WriteLine(header);
            sw.Close();
        }

        //Append the new score to what is already in the file
        StreamWriter appendSW = File.AppendText(saveFile);
        appendSW.WriteLine($"{shopPoints},{cutPoints},{assemblePoints},{totalPoints},{finalTime}");
        appendSW.Close();
    }

    #endregion

    /// <summary>
    /// The total time the assembly phase took
    /// </summary>
    public float FinalTime
    {
        get { return finalTime; }
        set { finalTime = value; }
    }
}