import { ListingDetail, ListingSummary } from "../../listings/models/listing.model";
import { UserSummary } from "../../../core/users/models/user.model";

export interface ListingGroup {
    id: string;
    seller: UserSummary;
    postingTime: Date
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