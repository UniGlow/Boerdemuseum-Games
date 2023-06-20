Example usage:

[SerializeField] private bool useBestOfFeature = false;
    [Tooltip("List of available Best-of-Versions. Players will be able to choose between these in the Lobby.")]
    [ConditionalHide("useBestOfFeature", true, false)]
    [SerializeField] private List<int> bestOfRange = new List<int>();
    [ConditionalHide("useBestOfFeature", true, true)]
    [Tooltip("List of available Match-Durations. If unlimited Matches should be possible add \"-1\" at the End!")]
    [SerializeField] private List<int> matchRange = new List<int>();