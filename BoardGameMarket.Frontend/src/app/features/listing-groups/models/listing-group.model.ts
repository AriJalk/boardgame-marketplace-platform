import { ListingDetail, ListingSummary } from "../../listings/models/listing.model";
import { UserSummary } from "../../users/models/user.model";

export interface ListingGroup {
    id: string;
    seller: UserSummary;
    pickupOptions: string;
    location: string;
    details?: string;
}

export interface ListingGroupSummary extends ListingGroup {
    listings: ListingSummary[];
}

export interface ListingGroupDetails extends ListingGroup {
    listings: ListingDetail[];
}