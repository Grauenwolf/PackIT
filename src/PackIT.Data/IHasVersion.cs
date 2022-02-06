namespace PackIT.Data
{
    /// <summary>
    /// Indicates that the entity has a Version property that must be incremented when saving.
    /// </summary>
    public interface IHasVersion
    {
        int Version { get; set; }
    }
}
