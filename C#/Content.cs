using System.IO;
using System.Drawing;

namespace LEdit
{
    public abstract class Content
    {
        public enum ContentType
        {
            Unknown,
            File,
            Monster,
            MonsterSprite,
        }

        public static string FormatID(int ID)
        {
            return ID.ToString("D4");
        }

        public static ContentType GetContentType(string Text)
        {
            switch (Text.ToLower())
            {
                case "file":
                    return ContentType.File;

                case "monster":
                    return ContentType.Monster;

                case "monstersprite":
                    return ContentType.MonsterSprite;

                default:
                    return ContentType.Unknown;
            }
        }

        public static string GetContentText(ContentType Type)
        {
            switch (Type)
            {
                case ContentType.File:
                    return "file";

                case ContentType.Monster:
                    return "monster";

                case ContentType.MonsterSprite:
                    return "monstersprite";

                default:
                    return string.Empty;
            }
        }

        public static string GetContentFolderName(ContentType Type)
        {
            switch (Type)
            {
                case ContentType.Monster:
                    return "monsters";

                case ContentType.MonsterSprite:
                    return "monstersprites";

                default:
                    return string.Empty;
            }
        }

        public static string GetFileExtension(ContentType Type)
        {
            switch (Type)
            {
                case ContentType.Monster:
                    return ".l2monster";

                case ContentType.MonsterSprite:
                    return ".l2monstersprite";

                default:
                    return ".l2";
            }
        }

        public static Content Retrieve(ContentType Type, int ID)
        {
            switch (Type)
            {
                case ContentType.Monster:
                    return L2Monster.Array[ID];

                case ContentType.MonsterSprite:
                    return L2MonsterSprite.Array[ID];

                default:
                    return null;
            }
        }

        public int ID = -1;

        public abstract string GetName();
        public abstract Bitmap GetIcon();

        public abstract void FromStream(BinaryReader br);
        public abstract void ToStream(BinaryWriter bw);

        public abstract void Load(FileInfo File);
        public abstract void Save(FileInfo File);
    }
}
