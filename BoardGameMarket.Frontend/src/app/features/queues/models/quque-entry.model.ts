import { UserSummary } from '../../../core/users/models/user.model';

export interface QueueEntry {
  id: string;
  status: string;
  buyer: UserSummary;
  createdAt: Date;
  priority?: number;
}

export interface QueuePageResponse {
  timeQueue: QueueEntry[];
  priorityQueue: QueueEntry[];
}
