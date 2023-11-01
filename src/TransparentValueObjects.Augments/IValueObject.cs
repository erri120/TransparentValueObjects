namespace TransparentValueObjects.Augments;

public interface IValueObject<TSelf, TValue>
    where TSelf : IValueObject<TSelf, TValue>
    where TValue : notnull { }
