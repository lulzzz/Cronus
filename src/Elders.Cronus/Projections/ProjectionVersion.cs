﻿using System.Runtime.Serialization;

namespace Elders.Cronus.Projections
{
    [DataContract(Name = "bb4883b9-c3a5-48e5-8ba1-28fb94d061ac")]
    public class ProjectionVersion : ValueObject<ProjectionVersion>
    {
        public ProjectionVersion(string projectionContractId, ProjectionStatus status, int revision, string hash)
        {
            ProjectionContractId = projectionContractId;
            Status = status;
            Revision = revision;
            Hash = hash;
        }

        [DataMember(Order = 1)]
        public string ProjectionContractId { get; private set; }

        [DataMember(Order = 2)]
        public ProjectionStatus Status { get; private set; }

        [DataMember(Order = 3)]
        public int Revision { get; private set; }

        [DataMember(Order = 4)]
        public string Hash { get; private set; }

        public ProjectionVersion WithStatus(ProjectionStatus status)
        {
            return new ProjectionVersion(ProjectionContractId, status, Revision, Hash);
        }

        public ProjectionVersion NextRevision()
        {
            return new ProjectionVersion(ProjectionContractId, Status, Revision + 1, Hash);
        }

        public override bool Equals(ProjectionVersion other)
        {
            if (ReferenceEquals(null, other)) return false;

            return
                string.Equals(Hash, other.Hash, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(ProjectionContractId, other.ProjectionContractId, System.StringComparison.OrdinalIgnoreCase) &&
                Revision == other.Revision;
        }

        public override int GetHashCode()
        {
            return Revision.GetHashCode();
        }

        public override string ToString()
        {
            return ProjectionContractId + "_" + Hash + "_" + Revision + "_" + Status;
        }
    }
}
