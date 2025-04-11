using System.Reflection;
using System.Text;
using static LiteDB5.Constants;

namespace LiteDB5
{
    /// <summary>
    /// The main exception for LiteDB
    /// </summary>
    public class LiteException : Exception
    {
        #region Errors code

        public const int FILE_NOT_FOUND = 101;
        public const int DATABASE_SHUTDOWN = 102;
        public const int INVALID_DATABASE = 103;
        public const int FILE_SIZE_EXCEEDED = 105;
        public const int COLLECTION_LIMIT_EXCEEDED = 106;
        public const int INDEX_DROP_ID = 108;
        public const int INDEX_DUPLICATE_KEY = 110;
        public const int INVALID_INDEX_KEY = 111;
        public const int INDEX_NOT_FOUND = 112;
        public const int INVALID_DBREF = 113;
        public const int LOCK_TIMEOUT = 120;
        public const int INVALID_COMMAND = 121;
        public const int ALREADY_EXISTS_COLLECTION_NAME = 122;
        public const int ALREADY_OPEN_DATAFILE = 124;
        public const int INVALID_TRANSACTION_STATE = 126;
        public const int INDEX_NAME_LIMIT_EXCEEDED = 128;
        public const int INVALID_INDEX_NAME = 129;
        public const int INVALID_COLLECTION_NAME = 130;
        public const int TEMP_ENGINE_ALREADY_DEFINED = 131;
        public const int INVALID_EXPRESSION_TYPE = 132;
        public const int COLLECTION_NOT_FOUND = 133;
        public const int COLLECTION_ALREADY_EXIST = 134;
        public const int INDEX_ALREADY_EXIST = 135;
        public const int INVALID_UPDATE_FIELD = 136;
        public const int ENGINE_DISPOSED = 137;

        public const int INVALID_FORMAT = 200;
        public const int DOCUMENT_MAX_DEPTH = 201;
        public const int INVALID_CTOR = 202;
        public const int UNEXPECTED_TOKEN = 203;
        public const int INVALID_DATA_TYPE = 204;
        public const int PROPERTY_NOT_MAPPED = 206;
        public const int INVALID_TYPED_NAME = 207;
        public const int PROPERTY_READ_WRITE = 209;
        public const int INITIALSIZE_CRYPTO_NOT_SUPPORTED = 210;
        public const int INVALID_INITIALSIZE = 211;
        public const int INVALID_NULL_CHAR_STRING = 212;
        public const int INVALID_FREE_SPACE_PAGE = 213;
        public const int DATA_TYPE_NOT_ASSIGNABLE = 214;
        public const int AVOID_USE_OF_PROCESS = 215;
        public const int NOT_ENCRYPTED = 216;
        public const int INVALID_PASSWORD = 217;
        public const int ILLEGAL_DESERIALIZATION_TYPE = 218;
        public const int ENTITY_INITIALIZATION_FAILED = 219;
        public const int MAPPER_NOT_FOUND = 220;
        public const int MAPPING_ERROR = 221;


        public const int INVALID_DATAFILE_STATE = 999;

        #endregion

        public int ErrorCode { get; private set; }
        public long Position { get; private set; }

        public LiteException(int code, string message)
            : base(message)
        {
            this.ErrorCode = code;
        }

        internal LiteException(int code, string message, params object[] args)
            : base(string.Format(message, args))
        {
            this.ErrorCode = code;
        }

        internal LiteException(int code, Exception inner, string message, params object[] args)
        : base(string.Format(message, args), inner)
        {
            this.ErrorCode = code;
        }

        internal static LiteException InvalidDatafileState(string message)
        {
            return new LiteException(INVALID_DATAFILE_STATE, message);
        }
    }
}