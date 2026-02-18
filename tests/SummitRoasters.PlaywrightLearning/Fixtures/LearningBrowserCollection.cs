namespace SummitRoasters.PlaywrightLearning.Fixtures;

/// <summary>
/// This tells xUnit to share ONE LearningBrowserFixture across all test classes
/// that are marked with [Collection("Learning")].
///
/// Without this, xUnit would create a new fixture per test class,
/// which means launching a new browser each time (slow!).
/// </summary>
[CollectionDefinition("Learning")]
public class LearningBrowserCollection : ICollectionFixture<LearningBrowserFixture>
{
}
