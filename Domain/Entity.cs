namespace Domain
{
    public interface IEntity
    {
    }

    public abstract class Entity<TId> : IEntity
    {
        private readonly TId _id;


        protected Entity()
        {

        }
        public Entity(long createdBy)
        {
            if (createdBy < 0)
                throw new ArgumentException($"{nameof(createdBy)} must be greater than zero.");

            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            UpdatedBy = createdBy;
            UpdatedOn = DateTime.UtcNow;
        }


        public Entity(long createdBy, TId id)
        {
            if (createdBy < 0)
                throw new ArgumentException($"{nameof(createdBy)} must be greater than zero.");

            _id = id;
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            UpdatedBy = createdBy;
            UpdatedOn = DateTime.UtcNow;
        }


        public TId Id => _id;
        public long CreatedBy { get; private set; }
        public long UpdatedBy { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        public void SetUpdatedBy(long updatedBy)
        {
            if (updatedBy < 0)
                throw new ArgumentException($"{nameof(updatedBy)} must be greater than zero.");
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}