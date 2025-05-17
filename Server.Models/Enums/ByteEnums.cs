namespace Server.Models.Enums;

public enum Roles : byte { SupAdmin = 1, Admin = 2, Lecturer = 3, Student = 4 }

public enum Semesters : byte { Both = 0, Fall = 1, Spring = 2, }

public enum CatalogTypes : byte { USC = 1, FSC = 2, }

public enum EduLevels : byte { Bachelor = 1, Master = 2, PHD = 3, }

public enum RecordStatus : byte { NotApproved = 0, Approved = 1, UnderSuspicious = 2, }
