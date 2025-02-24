import { Injectable } from '@angular/core';
import { CreateReviewCommand, ReviewByProductIdWithPaginationDto } from '../../web-api-client';
import { Review } from '../../interfaces/review';
import { ReviewsList } from '../../interfaces/list';
import { Observable, of } from 'rxjs';
import * as momentJalaali from "moment-jalaali";

@Injectable({
  providedIn: 'root'
})
export class ReviewmapperService {

  constructor() { }



  //#region FrontToDatabase
  mapReview(review: Review, productId: number, customerId?: string) {
    let reviewCommand = new CreateReviewCommand();
    reviewCommand.rating = review.rating;
    reviewCommand.content = review.content ?? '';
    reviewCommand.authorName = review.author ?? '';
    reviewCommand.productId = productId;
    reviewCommand.customerId = customerId;
    
    return reviewCommand;
  }
  //#endregion

  //#region DatabaseToFront
  mapReviewdDto(reviewDto: ReviewByProductIdWithPaginationDto) {
    let review = new Review();
    review.id = reviewDto.id ?? 0;
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    review.date = momentJalaali(reviewDto.date).format("jD jMMMM jYYYY");
    review.author = reviewDto.authorName ?? '';
    review.avatar = reviewDto.customer?.avatar ?? '';
    review.rating = reviewDto.rating ?? 0;
    review.content = reviewDto.content ?? '';
    return review;
  }
  mapReviews(reviewDtos: ReviewByProductIdWithPaginationDto[]) {
    let reviews: Review[] = [];
    reviewDtos?.forEach(l => {
      let review = new Review();
      review = this.mapReviewdDto(l);
      reviews.push(review);
    })
    return reviews;
  }
  getProductReviews(reviewDtos: ReviewByProductIdWithPaginationDto[], pageArg: number, limitArg: number, sortArg?: string): Observable<ReviewsList> {
    let items = this.mapReviews(reviewDtos.slice(0));

    items.sort((a, b) => {
      if (a.date > b.date) {
        return -1;
      }
      if (a.date < b.date) {
        return 1;
      }

      return 0;
    });

    const page = pageArg || 1;
    const limit = limitArg || 8;
    const sort = sortArg || 'default';
    const total = items.length;
    const pages = Math.ceil(items.length / limit);
    const from = (page - 1) * limit + 1;
    const to = page * limit;

    items = items.slice(from - 1, to) as unknown as Array<Review>;

    return of({
      items,
      page,
      limit,
      sort,
      total,
      pages,
      from,
      to,
    });
  }
  //#endregion
}

