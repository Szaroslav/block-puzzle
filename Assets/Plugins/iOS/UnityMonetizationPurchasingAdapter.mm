#import <UnityAds/USRVUnityPurchasing.h>

#import <il2cpp-api-types.h>
#import "UnityAdsUtilities.h"
#import "UnityJsonAdditions.h"

extern "C" {
struct ILUnityPurchasingProduct {
    Il2CppChar* productId;
    Il2CppChar* localizedTitle;
    Il2CppChar* localizedDescription;
    Il2CppChar* localizedPriceString;
    Il2CppChar* isoCurrencyCode;
    Il2CppChar* productType;
    double localizedPrice;
};

struct ILUnityPurchasingPurchaseCallbacks {
    const void* completionHandler;
    const void* errorHandler;
};

// Callback called to C# that handles retrieiving the products.
// It is assumed that pDelegate will be the same delegate passed into
// UnityPurchasingInvokeRetrieveProductsCallback.
typedef void (*UnityPurchasingOnRetrieveProductsCallback)(const void *pDelegate);
UnityPurchasingOnRetrieveProductsCallback unityPurchasingOnRetrieveProductsCallback;

// Callback called to C# that handles the purchasing flow.
// It is assumed that pDelegate will be the same delegate passed into
// UnityPurchasingInvokeTransactionCompleteCallback and
// UnityPurchasingInvokeTransactionErrorCallback
typedef void (*UnityPurchasingOnPurchaseCallback)(const char *productId, struct ILUnityPurchasingPurchaseCallbacks* callbacks);
UnityPurchasingOnPurchaseCallback unityPurchasingOnPurchaseCallback;

struct UnityPurchasingAdapterCallbacks {
    UnityPurchasingOnRetrieveProductsCallback unityPurchasingOnRetrieveProductsCallback;
    UnityPurchasingOnPurchaseCallback unityPurchasingOnPurchaseCallback;
};
}

@interface UnityPurchasingAdapterDelegate : NSObject <USRVUnityPurchasingDelegate>
@end

@implementation UnityPurchasingAdapterDelegate
-(void)loadProducts:(void (^)(NSArray<UPURProduct*> *products))completionHandler {
    unityPurchasingOnRetrieveProductsCallback(CFBridgingRetain(completionHandler));
}
-(void)purchaseProduct:(NSString *)productId completionHandler:(UnityPurchasingTransactionCompletionHandler)completionHandler errorHandler:(UnityPurchasingTransactionErrorHandler)errorHandler userInfo:(nullable NSDictionary *)extras {
    struct ILUnityPurchasingPurchaseCallbacks* callbacks = new ILUnityPurchasingPurchaseCallbacks{};
    callbacks->completionHandler = CFBridgingRetain(completionHandler);
    callbacks->errorHandler = CFBridgingRetain(errorHandler);
    unityPurchasingOnPurchaseCallback([productId UTF8String], callbacks);
}

@end

static id <USRVUnityPurchasingDelegate> unityPurchasingAdapterDelegate;
extern "C" {
/**
* Sets the callbacks for invoking purchasing adapter functionality into C#.
*/
void UnityPurchasingSetPurchasingAdapterCallbacks(struct UnityPurchasingAdapterCallbacks *callbacks) {
    unityPurchasingOnRetrieveProductsCallback = callbacks->unityPurchasingOnRetrieveProductsCallback;
    unityPurchasingOnPurchaseCallback = callbacks->unityPurchasingOnPurchaseCallback;
    if (unityPurchasingAdapterDelegate == NULL) {
        unityPurchasingAdapterDelegate = [[UnityPurchasingAdapterDelegate alloc] init];
        [USRVUnityPurchasing setDelegate:unityPurchasingAdapterDelegate];
    }
}

/**
* Allocates a fixed sized array to be returned as a pointer to C#.
*/
const void *UnityPurchasingAdapterAllocateProductsArray(int num) {
    NSMutableArray *array = [NSMutableArray arrayWithCapacity:num];
    return CFBridgingRetain(array);
}

/**
* Appends the given product to the end of the product array.
*/
void UnityPurchasingAddItemToProductsArray(const void *pArray, struct ILUnityPurchasingProduct *pProduct) {
    NSMutableArray *array = (__bridge NSMutableArray *) pArray;
    UPURProduct *product = [UPURProduct build:^(UPURProductBuilder* builder) {
        builder.productId = NSStringFromIl2CppString(pProduct->productId);
        builder.localizedTitle = NSStringFromIl2CppString(pProduct->localizedTitle);
        builder.localizedDescription = NSStringFromIl2CppString(pProduct->localizedDescription);
        builder.localizedPriceString = NSStringFromIl2CppString(pProduct->localizedPriceString);
        builder.isoCurrencyCode = NSStringFromIl2CppString(pProduct->isoCurrencyCode);
        builder.productType = NSStringFromIl2CppString(pProduct->productType);
        builder.localizedPrice = [[NSDecimalNumber alloc] initWithDouble:pProduct->localizedPrice];
    }];
    [array addObject:product];
}

/**
* Invokes the given retrieve products delegate with the given products.
*/
void UnityPurchasingInvokeRetrieveProductsCallback(const void *pDelegate, const void *pProducts) {
    if (pDelegate != NULL && pProducts != NULL) {
        NSArray *products = (__bridge NSArray *) pProducts;
        UnityPurchasingLoadProductsCompletionHandler completionHandler = (__bridge UnityPurchasingLoadProductsCompletionHandler)pDelegate;
        completionHandler(products);
        CFBridgingRelease(pDelegate);
        CFBridgingRelease(pProducts);
    }
}

/**
* Invokes the given transaction delegate's complete callback with the given details
*/
void UnityPurchasingInvokeTransactionCompleteCallback(struct ILUnityPurchasingPurchaseCallbacks* callbacks, Il2CppChar* transactionDetailsJson) {
    if (callbacks != NULL && transactionDetailsJson != NULL) {
        NSString *transactionDetailsJsonString = NSStringFromIl2CppString(transactionDetailsJson);
        NSError *error = nil;
        UPURTransactionDetails *details = [UPURTransactionDetails buildWithJson:transactionDetailsJsonString error:&error];
        if (error) {
            // do nothing
        } else if (details) {
            // make sure details is non-null
            UnityPurchasingTransactionCompletionHandler handler = (__bridge UnityPurchasingTransactionCompletionHandler)(callbacks->completionHandler);
            handler(details);
        }
        CFBridgingRelease(callbacks->completionHandler);
        CFBridgingRelease(callbacks->errorHandler);
        free(callbacks);
    }
}

/**
* Invokes the given transaction delegate's error callback with the given error and message.
*/
void UnityPurchasingInvokeTransactionErrorCallback(struct ILUnityPurchasingPurchaseCallbacks* callbacks, Il2CppChar* transactionErrorDetailsJson) {
    if (callbacks != NULL && transactionErrorDetailsJson != NULL) {
        NSString *transactionErrorDetailsJsonString = NSStringFromIl2CppString(transactionErrorDetailsJson);
        NSError *error = nil;
        UPURTransactionErrorDetails *details = [UPURTransactionErrorDetails buildWithJson: transactionErrorDetailsJsonString error:&error];
        if (error) {
            // do nothing
        } else if (details) {
            // make sure details is non-null
            UnityPurchasingTransactionErrorHandler handler = (__bridge UnityPurchasingTransactionErrorHandler)(callbacks->errorHandler);
            handler(details);
        }
        CFBridgingRelease(callbacks->completionHandler);
        CFBridgingRelease(callbacks->errorHandler);
        free(callbacks);
    }
}
}

