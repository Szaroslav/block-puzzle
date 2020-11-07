#import <stdio.h>
#import <il2cpp-api-types.h>

inline const char * UnityAdsCopyString(const char * string) {
    char * copy = (char *)malloc(strlen(string) + 1);
    strcpy(copy, string);
    return copy;
}

/**
 * Returns the size of an Il2CppString
 */
inline size_t Il2CppStringLen(const Il2CppChar* str) {
    const Il2CppChar* start = str;
    while (*str) ++str;
    return str - start;
}

/**
 * Converts an Il2CppChar string to an NSString
 */
inline NSString* NSStringFromIl2CppString(const Il2CppChar* str) {
    size_t len = Il2CppStringLen(str);
    return [[NSString alloc] initWithBytes:(const void*)str
                                    length:sizeof(Il2CppChar) * len
                                  encoding:NSUTF16LittleEndianStringEncoding];
}

/**
 * Converts an NSString to an Il2CppChar string.
 *
 * Note when passing back to C#, this data will be freed by il2cpp.
 */
inline Il2CppChar* Il2CppStringFromNSString(NSString* str) {
    size_t len = str.length;
    NSData* cStr = [str dataUsingEncoding:NSUTF16LittleEndianStringEncoding];
    Il2CppChar* buffer = new Il2CppChar[len + 1];
    memset(buffer, 0, (len + 1) * sizeof(Il2CppChar));
    [cStr getBytes:buffer length:len * sizeof(Il2CppChar)];
    return buffer;
}

/**
 * Converts a UTF16-LE NSData* to an Il2CppChar string.
 *
 * Note when passing back to C#, this data will be freed by il2cpp.
 */
inline Il2CppChar* Il2CppStringFromNSData(NSData* data) {
    size_t len = data.length / sizeof(Il2CppChar);
    Il2CppChar* buffer = new Il2CppChar[len + 1];
    memset(buffer, 0, (len + 1) * sizeof(Il2CppChar));
    [data getBytes:buffer length:len * sizeof(Il2CppChar)];
    return buffer;
}
