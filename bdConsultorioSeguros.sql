USE [PruebaV1]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Asegurados]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asegurados](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [nvarchar](max) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[Edad] [int] NOT NULL,
	[Correo] [nvarchar](max) NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Asegurados] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AseguradosSeguros]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AseguradosSeguros](
	[AseguradosId] [int] NOT NULL,
	[SegurosId] [int] NOT NULL,
	[Estado] [nvarchar](50) NULL,
 CONSTRAINT [PK_AseguradosSeguros] PRIMARY KEY CLUSTERED 
(
	[AseguradosId] ASC,
	[SegurosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seguros]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seguros](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Codigo] [nvarchar](max) NOT NULL,
	[SumaAsegurada] [decimal](18, 2) NOT NULL,
	[Prima] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](max) NOT NULL,
	[Ramo] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Seguros] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asegurados] ADD  DEFAULT (N'') FOR [Correo]
GO
ALTER TABLE [dbo].[Asegurados] ADD  DEFAULT (N'') FOR [Estado]
GO
ALTER TABLE [dbo].[Seguros] ADD  DEFAULT (N'') FOR [Estado]
GO
ALTER TABLE [dbo].[Seguros] ADD  DEFAULT (N'') FOR [Ramo]
GO
ALTER TABLE [dbo].[AseguradosSeguros]  WITH CHECK ADD  CONSTRAINT [FK_AseguradosSeguros_Asegurados_AseguradosId] FOREIGN KEY([AseguradosId])
REFERENCES [dbo].[Asegurados] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AseguradosSeguros] CHECK CONSTRAINT [FK_AseguradosSeguros_Asegurados_AseguradosId]
GO
ALTER TABLE [dbo].[AseguradosSeguros]  WITH CHECK ADD  CONSTRAINT [FK_AseguradosSeguros_Seguros_SegurosId] FOREIGN KEY([SegurosId])
REFERENCES [dbo].[Seguros] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AseguradosSeguros] CHECK CONSTRAINT [FK_AseguradosSeguros_Seguros_SegurosId]
GO
/****** Object:  StoredProcedure [dbo].[AddAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddAsegurado]  
	@Cedula NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Telefono NVARCHAR(50),
    @Edad INT,
	@Correo NVARCHAR(50),
	@Estado NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;


    IF EXISTS (SELECT 1 FROM Asegurados WHERE Cedula = @Cedula)
    BEGIN
				
				IF EXISTS (SELECT 1 FROM Asegurados WHERE Cedula = @Cedula AND Estado = 'A')
				BEGIN
					SELECT 'Ya existe un asegurado con esa cédula' AS Message, 1 AS Error
					RETURN
				END
				ELSE 
				UPDATE Asegurados SET Cedula = @Cedula, Nombre = @Nombre, Telefono = @Telefono, Edad = @Edad, Correo = @Correo, Estado = 'A' WHERE Cedula = @Cedula
        SELECT 'Asegurado registrado' AS Message, 0 AS Error
        RETURN
    END

    INSERT INTO Asegurados (Cedula, Nombre, Telefono, Edad, Correo, Estado)
    VALUES (@Cedula, @Nombre, @Telefono, @Edad, @Correo, @Estado)


    IF @@ROWCOUNT = 0
    BEGIN
        -- Return an error message
        SELECT 'No se pudo insertar el registro' AS Message, 1 AS Error
        RETURN
    END

    -- Return success message
    SELECT 'Registro exitoso' AS Message, 0 AS Error
END
GO
/****** Object:  StoredProcedure [dbo].[AssignSegurosToAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AssignSegurosToAsegurado]
    @IdAsegurado INT,
    @IdSeguro INT
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM Seguros WHERE Id = @IdSeguro)
    BEGIN
        SELECT 'El seguro no existe.' AS Message, 1 AS Error;
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM AseguradosSeguros WHERE AseguradosId = @IdAsegurado AND SegurosId = @IdSeguro)
    BEGIN
        IF (SELECT Estado FROM AseguradosSeguros WHERE AseguradosId = @IdAsegurado AND SegurosId = @IdSeguro) = 'I'
        BEGIN
            -- Si el estado es 'I', actualizar el estado del seguro existente a 'A'
            UPDATE AseguradosSeguros
            SET Estado = 'A'
            WHERE AseguradosId = @IdAsegurado AND SegurosId = @IdSeguro;

            SELECT 'Seguro agregado' AS Message, 0 AS Error;
            RETURN;
        END
        ELSE
        BEGIN
            SELECT 'El asegurado ya tiene un seguro activo con el seguro seleccionado.' AS Message, 1 AS Error;
            RETURN;
        END
    END
    ELSE
    BEGIN
        -- Si no existe, insertar el nuevo seguro
        INSERT INTO AseguradosSeguros (AseguradosId, SegurosId, Estado)
        VALUES (@IdAsegurado, @IdSeguro, 'A'); -- Asumiendo que 'A' es el estado activo

        SELECT 'Seguro agregado' AS Message, 0 AS Error;
        RETURN;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAsegurado]
	@Id INT,
	@Estado NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM AseguradosSeguros WHERE AseguradosId = @Id AND Estado != 'I')
	BEGIN
		SELECT 'El asegurado tiene seguros activos' AS Message, 1 AS Error
        RETURN
	END

	IF EXISTS (SELECT 1 FROM Asegurados WHERE Id = @Id)
    BEGIN

        UPDATE Asegurados SET Estado = @Estado WHERE Id = @Id

		IF @@ROWCOUNT = 0
		BEGIN
			SELECT 'No se pudo eliminar el asegurado' AS Message, 1 AS Error
			RETURN
		END


		SELECT 'Eliminacion exitosa' AS Message, 0 AS Error

    END
	ELSE
	BEGIN

        SELECT 'No se encontró el registro' AS Message, 1 AS Error
        RETURN
    END
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteSegurosToAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteSegurosToAsegurado] 
	@IdSeguro INT,
	@Estado NVARCHAR(50),
	@IdAsegurado INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM AseguradosSeguros WHERE AseguradosId = @IdAsegurado AND SegurosId = @IdSeguro)
    BEGIN
        UPDATE AseguradosSeguros SET Estado = @Estado WHERE AseguradosId = @IdAsegurado AND SegurosId = @IdSeguro
		SELECT 'Se elimino el seguro del asegurado' AS Message, 0 AS Error
        RETURN
    END
    ELSE
    BEGIN
		SELECT 'No se encontraron seguros activos' AS Message, 1 AS Error
        RETURN
    END
END
GO
/****** Object:  StoredProcedure [dbo].[GetAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAsegurado]
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Asegurados WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[GetAsegurados]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetAsegurados]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM Asegurados WHERE Estado = 'A'
END
GO
/****** Object:  StoredProcedure [dbo].[GetSegurosByCedula]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSegurosByCedula] 
	-- Add the parameters for the stored procedure here
	@Cedula NVARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON;


      SELECT s.*, a.*
    FROM Asegurados a
    INNER JOIN AseguradosSeguros asg ON a.Id = asg.AseguradosId
    INNER JOIN Seguros s ON asg.SegurosId = s.Id
    WHERE a.Cedula = @Cedula AND
	asg.Estado = 'A'
END
GO
/****** Object:  StoredProcedure [dbo].[GetSegurosById]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetSegurosById] 
	-- Add the parameters for the stored procedure here
	@Id INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT s.*
    FROM Asegurados a
    INNER JOIN AseguradosSeguros asg ON a.Id = asg.AseguradosId
    INNER JOIN Seguros s ON asg.SegurosId = s.Id
    WHERE a.Id = @Id AND
	asg.Estado = 'A'
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateAsegurado]    Script Date: 29/12/2023 12:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateAsegurado] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@Cedula NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @Telefono NVARCHAR(50),
    @Edad INT,
	@Correo NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM Asegurados WHERE Cedula = @Cedula And Id != @Id)
    BEGIN

        SELECT 'Ya existe un asegurado con esa cédula' AS Message, 1 AS Error
        RETURN
    END

	IF EXISTS (SELECT 1 FROM Asegurados WHERE Id = @Id)
    BEGIN

        UPDATE Asegurados SET Cedula = @Cedula, Nombre = @Nombre, Telefono = @Telefono, Edad = @Edad, Correo = @Correo WHERE Id = @Id


		IF @@ROWCOUNT = 0
		BEGIN
			-- Return an error message
			SELECT 'No se pudo actualizar el registro' AS Message, 1 AS Error
			RETURN
		END

		-- Return success message
		SELECT 'Actualizacion exitosa' AS Message, 0 AS Error
    END
	ELSE
	BEGIN

        SELECT 'No se encontró el registro' AS Message, 1 AS Error
        RETURN
    END

	
END
GO
