using DemoFuncionario.Core;

namespace DemoFuncionario.Tests.Core; 

public class AggregateBehavior {
    [Fact]
    public void Equals_when_both_have_same_id_and_version() {
        var agg1 = new DemoAggregate(1, new byte[] { 1, 2, 3 });
        var agg2 = new DemoAggregate(1, new byte[] { 1, 2, 3 });
        
        Assert.Equal(agg1, agg2);
    }
    
    [Fact]
    public void Different_when_both_have_same_id_but_different_version() {
        var agg1 = new DemoAggregate(1, new byte[] { 1, 2, 3 });
        var agg2 = new DemoAggregate(1, new byte[] { 4, 2, 3 });
        
        Assert.NotEqual(agg1, agg2);
    }
    
    class DemoAggregate: Aggregate {
        private readonly int _id;
        private readonly byte[]? _version;

        public DemoAggregate(int id = 1, byte[]? version = default) {
            _id = id;
            _version = version;
        }

        public override int Id => _id;
        public override byte[]? Version => _version;
    }
}
