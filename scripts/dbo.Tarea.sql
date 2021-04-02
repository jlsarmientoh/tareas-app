CREATE TABLE [dbo].[Tareas]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(100) NULL, 
    [IsComplete] BIT NULL, 
    [Owner] BIGINT NULL, 
    CONSTRAINT [FK_Tarea_User] FOREIGN KEY ([Owner]) REFERENCES [User]([UsuarioId])
)
