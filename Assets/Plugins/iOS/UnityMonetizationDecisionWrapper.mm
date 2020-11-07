#import <UnityAds/UnityMonetization.h>
#import <Unity/UnityInterface.h>
#import <il2cpp-api-types.h>
#import "UnityAdsUtilities.h"
#import "UnityJsonAdditions.h"

extern "C" {
typedef void (*UnityMonetizationShowAdStartCallback)();
typedef void (*UnityMonetizationShowAdFinishCallback)(int finishState);
}

@interface UnityMonetizationUnityShowAdDelegate : NSObject<UMONShowAdDelegate>
@property (nonatomic) UnityMonetizationShowAdStartCallback startCallback;
@property (nonatomic) UnityMonetizationShowAdFinishCallback finishCallback;

-(instancetype)initWithCallbacks:(UnityMonetizationShowAdStartCallback)startCallback finishCallback:(UnityMonetizationShowAdFinishCallback)finishCallback;
@end

@implementation UnityMonetizationUnityShowAdDelegate
- (instancetype)initWithCallbacks:(UnityMonetizationShowAdStartCallback)startCallback finishCallback:(UnityMonetizationShowAdFinishCallback)finishCallback {
    if (self = [super init]) {
        self.startCallback = startCallback;
        self.finishCallback = finishCallback;
    }
    return self;
}
-(void)unityAdsDidFinish:(NSString *)placementId withFinishState:(UnityAdsFinishState)finishState {
    UnityPause(0);
    if (self.finishCallback) {
        self.finishCallback(finishState);
    }
}
-(void)unityAdsDidStart:(NSString *)placementId {
    if (self.startCallback) {
        self.startCallback();
    }
}
@end

const Il2CppChar* serializeJsonToIl2CppString(NSDictionary* dict) {
    NSError* error;
    NSData* data = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&error];
    NSString* str = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    if (data != nil) {
        return Il2CppStringFromNSString(str);
    }
    return NULL;
}

NSDictionary* getJsonDictionaryFromItem(UMONItem* item) {
    return @{
             @"itemType": item.type ? item.type : [NSNull null],
             @"productId": item.productId ? item.productId : [NSNull null],
             @"quantity": @(item.quantity)
    };
}

NSArray* getJsonArrayFromItemArray(NSArray<UMONItem*>* items) {
    NSMutableArray* array = [[NSMutableArray alloc] init];
    for (UMONItem* item in items) {
        [array addObject:getJsonDictionaryFromItem(item)];
    }
    return [array copy];
}

BOOL isValidPrice(NSDecimalNumber* number) {
    return number && ![number isEqualToNumber:[NSDecimalNumber notANumber]];
}

NSDictionary* getJsonDictionaryFromProduct(UPURProduct* product) {
    return @{
             @"productId": product.productId ? product.productId : [NSNull null],
             @"localizedTitle": product.localizedTitle ? product.localizedTitle : [NSNull null],
             @"localizedDescription": product.localizedDescription ? product.localizedDescription : [NSNull null],
             @"localizedPriceString": product.localizedPriceString ? product.localizedPriceString : [NSNull null],
             @"isoCurrencyCode": product.isoCurrencyCode ? product.isoCurrencyCode : [NSNull null],
             @"localizedPrice": isValidPrice(product.localizedPrice) ? product.localizedPrice : [NSNull null],
             @"productType": product.productType ? product.productType : [NSNull null]
    };
}

NSDictionary* getPromoMetadataDictionary(UMONPromoMetaData* metadata) {
    return @{
             @"impressionDate": metadata.impressionDate ? @([metadata.impressionDate timeIntervalSince1970] * 1000) : [NSNull null],
             @"offerDuration": @(metadata.offerDuration),
             @"costs": getJsonArrayFromItemArray(metadata.costs),
             @"payouts": getJsonArrayFromItemArray(metadata.payouts),
             @"premiumProduct": getJsonDictionaryFromProduct(metadata.premiumProduct)
    };
}

const Il2CppChar* serializePromoMetadataToJson(UMONPromoMetaData* metadata) {
    NSDictionary* dict = getPromoMetadataDictionary(metadata);
    return serializeJsonToIl2CppString([dict copy]);
}



extern "C" {
bool UnityMonetizationPlacementContentIsReady(const void* pPlacementContent) {
    UMONPlacementContent* placementContent = (__bridge UMONPlacementContent*)pPlacementContent;
    return placementContent.ready;
}

bool UnityMonetizationPlacementContentSendCustomEvent(const void* pPlacementContent, const Il2CppChar* customEventJson) {
    NSString *customEventJsonString = NSStringFromIl2CppString(customEventJson);
    NSError *error = nil;
    UMONCustomEvent *event = [UMONCustomEvent buildWithJson:customEventJsonString error:&error];
    if (error) {
        // do nothing
        NSLog(@"UnityMonetizationPlacementContentSendCustomEvent error occurred : %@", [error description]);
        return false;
    } else if (event) {
        // make sure details is non-null
        UMONPlacementContent* placementContent = (__bridge UMONPlacementContent*)pPlacementContent;
        [placementContent sendCustomEvent:event];
        return true;
    } else {
        NSLog(@"UnityMonetizationPlacementContentSendCustomEvent was not able to send event");
        return false;
    }
}

const Il2CppChar* UnityMonetizationGetPlacementContentExtras(const void* pPlacementContent) {
    if (pPlacementContent) {
        UMONPlacementContent* placementContent = (__bridge UMONPlacementContent*) pPlacementContent;
        NSDictionary* dict = placementContent.userInfo;
        if (dict != nil) {
            return serializeJsonToIl2CppString(dict);
        }
    }
    return NULL;
}

bool UnityMonetizationPlacementContentIsRewarded(const void* pPlacementContent) {
    UMONRewardablePlacementContent* placementContent = (__bridge UMONRewardablePlacementContent*)pPlacementContent;
    return placementContent.rewarded;
}

const Il2CppChar* UnityMonetizationPlacementContentGetRewardId(const void* pPlacementContent) {
    UMONRewardablePlacementContent* placementContent = (__bridge UMONRewardablePlacementContent*)pPlacementContent;
    return Il2CppStringFromNSString(placementContent.rewardId);
}

void UnityMonetizationPlacementContentShowAd(const void* pPlacementContent, UnityMonetizationShowAdStartCallback startCallback, UnityMonetizationShowAdFinishCallback finishCallback) {
    UMONShowAdPlacementContent* placementContent = (__bridge UMONShowAdPlacementContent*)pPlacementContent;
    UnityPause(1);
    [placementContent show:UnityGetGLViewController() withDelegate:[[UnityMonetizationUnityShowAdDelegate alloc] initWithCallbacks:startCallback finishCallback:finishCallback]];
}

const Il2CppChar* UnityMonetizationGetPromoAdMetadata(const void* pPlacementContent) {
    if (pPlacementContent) {
        UMONPromoAdPlacementContent* placementContent = (__bridge UMONPromoAdPlacementContent*)pPlacementContent;
        return serializePromoMetadataToJson(placementContent.metadata);
    }
    return NULL;
}

const char* UnityMonetizationGetPlacementContentType(const void* pPlacementContent) {
    UMONPlacementContent* placementContent = (__bridge UMONPlacementContent*)pPlacementContent;
    // NOTE: il2cpp will free this pointer after invocation!
    return UnityAdsCopyString([placementContent.type UTF8String]);
}

int UnityMonetizationGetPlacementContentState(const void* pPlacementContent) {
    UMONPlacementContent* placementContent = (__bridge UMONPlacementContent*)pPlacementContent;
    return placementContent.state;
}

void UnityMonetizationPlacementContentReleaseReference(const void* pPlacementContent) {
    CFBridgingRelease(pPlacementContent);
}

}
